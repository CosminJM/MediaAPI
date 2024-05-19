using Media.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Media.DataAccess
{
    public class MediaContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public MediaContext() { }

        public MediaContext(DbContextOptions<MediaContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }

        public DbSet<Channel> Channels { get; set; }
        public DbSet<Video> Videos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer("Data Source = (localdb)\\MSSQLLocalDB; Initial Catalog = MediaDB")
            //    .LogTo(Console.WriteLine)
            //    .EnableSensitiveDataLogging();

            //var connectionString = _configuration.GetConnectionString("MediaDB") ?? "DefaultConnectionString";
            //optionsBuilder.UseSqlServer(connectionString)
            //    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
            //    .LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }

        public override int SaveChanges()
        {
            UpdateAddedDateTime();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateAddedDateTime();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateAddedDateTime()
        {
            var addedEntries = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added)
                .Select(e => e.Entity)
                .OfType<Video>();

            var addedDateTime = DateTime.UtcNow;

            foreach (var entry in addedEntries)
            {
                entry.AddedDateTime = addedDateTime;
            }
        }
    }
}