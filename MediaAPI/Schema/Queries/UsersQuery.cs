using AutoMapper;
using HotChocolate.Authorization;
using Media.DataAccess;
using Media.Domain;
using MediaAPI.Middlewares;
using MediaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace MediaAPI.Schema.Queries
{
    [ExtendObjectType(typeof(Query))]
    public class UsersQuery
    {
        private readonly IMapper _mapper;

        public UsersQuery(IMapper mapper)
        {
            _mapper = mapper;
        }

        [Authorize]
        [UseUser]
        [UseDbContext(typeof(MediaContext))]
        public async Task<UserDto> GetUser([User] User user, [Service] IDbContextFactory<MediaContext> contextFactory)
        {
            using (var context = contextFactory.CreateDbContext())
            {
                var userFromDb = await context.Users.FirstOrDefaultAsync(u => u.Username == user.Username);
                var userDto = _mapper.Map<UserDto>(userFromDb);
                return userDto;
            }
        }
    }
}
