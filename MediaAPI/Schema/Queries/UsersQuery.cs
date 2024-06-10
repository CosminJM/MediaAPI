using AutoMapper;
using Media.DataAccess;
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

        [UseDbContext(typeof(MediaContext))]
        public async Task<UserDto> GetUser(Guid userId, [Service] IDbContextFactory<MediaContext> contextFactory)
        {
            using (var context = contextFactory.CreateDbContext())
            {
                var user = await context.Users.FirstOrDefaultAsync(u => u.Id == userId);
                var userDto = _mapper.Map<UserDto>(user);
                return userDto;
            }
        }
    }
}
