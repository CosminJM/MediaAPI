using Media.DataAccess;
using Media.DataAccess.Repository;
using MediaAPI.Models;
using MediaAPI.Schema.Mutations;
using MediaAPI.Schema.Queries;
using MediaAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace MediaAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers(options =>
            {
                options.ReturnHttpNotAcceptable = true;
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            builder.Services.AddGraphQLServer()
                .AddQueryType<Query>()
                .AddTypeExtension<UsersQuery>()
                .AddTypeExtension<ChannelsQuery>()
                .AddMutationType<Mutation>()
                .AddTypeExtension<ChannelMutation>()
                // Used types as arguments on queries and mutation must be specified
                .AddType<ChannelForCreationDto>()
                .AddType<ChannelForUpdateDto>()
                .AddAuthorization();

            builder.Services.AddPooledDbContextFactory<MediaContext>(
                dbContextOptions => dbContextOptions.UseSqlServer(builder.Configuration.GetConnectionString("MediaDB"))
                .EnableSensitiveDataLogging()
                .UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll)
                .LogTo(Console.WriteLine)
                );

            //Mapper
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            builder.Services.AddTransient<IChannelsRepository, ChannelsRepository>();
            builder.Services.AddTransient<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IAuthService, AuthService>();

            var jwtSettings = builder.Configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"];

            builder.Services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                };
            });

            builder.Services.AddAuthorization();

            builder.Services.AddCors(corsOptions =>
            {
                corsOptions.AddPolicy("AllowPolicy", policy =>
                {
                    policy.WithOrigins("http://localhost:9000", "https://localhost:7000").AllowAnyHeader().AllowAnyMethod().AllowCredentials();
                });
            });

            var app = builder.Build();

            MigrateDb(app.Services);

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("AllowPolicy");

            //app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            //For GraphQL Subscriptions
            app.UseWebSockets();

            app.MapGraphQL();

            app.MapControllers();

            app.Run();
        }

        private static void MigrateDb(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var dbContextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<MediaContext>>();

                using (var dbContext = dbContextFactory.CreateDbContext())
                {
                    dbContext.Database.Migrate();
                }
            }
        }
    }
}
