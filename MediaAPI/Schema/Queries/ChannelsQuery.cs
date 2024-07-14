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
    public class ChannelsQuery
    {
        private readonly IMapper _mapper;

        public ChannelsQuery(IMapper mapper)
        {
            _mapper = mapper;
        }

        [Authorize]
        [UseDbContext(typeof(MediaContext))]
        [UsePaging(IncludeTotalCount = true, DefaultPageSize = 5, AllowBackwardPagination = true, MaxPageSize = 100)]
        [UseUser]
        public IQueryable<ChannelDto> GetPaginatedChannels([User] User user, [Service] IDbContextFactory<MediaContext> contextFactory, string? search)
        {
            var context = contextFactory.CreateDbContext();
            var channels = context.Channels.Where(c => c.Users.Any(u => u.Username == user.Username) && (string.IsNullOrEmpty(search)/*return true (all values for user) if search is empty */ || c.Name.Contains(search) || c.ChannelIdentificator.Contains(search)))
                .OrderBy(c => c.Name).Select(c => _mapper.Map<ChannelDto>(c));

            return channels;
        }
    }
}
