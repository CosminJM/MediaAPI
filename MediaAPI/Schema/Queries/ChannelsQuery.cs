using AutoMapper;
using HotChocolate.Authorization;
using Media.DataAccess;
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
        public IQueryable<ChannelDto> GetPaginatedChannels([Service] IDbContextFactory<MediaContext> contextFactory)
        {
            var context = contextFactory.CreateDbContext();
            var channels = context.Channels.OrderBy(c => c.Name).Select(c => _mapper.Map<ChannelDto>(c));

            return channels;
        }
    }
}
