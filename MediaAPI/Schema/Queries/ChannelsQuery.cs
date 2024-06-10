using AutoMapper;
using HotChocolate.Authorization;
using Media.DataAccess;
using Media.DataAccess.Repository;
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
        public IQueryable<ChannelDto> GetChannels([Service] IDbContextFactory<MediaContext> contextFactory)
        {
            var context = contextFactory.CreateDbContext();
            var channels = context.Channels.Select(c => _mapper.Map<ChannelDto>(c));

            return channels; 
        }
    }
}
