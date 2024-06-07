using AutoMapper;
using Media.DataAccess;
using Media.DataAccess.Repository;
using MediaAPI.Models;

namespace MediaAPI.Schema.Queries
{
    public class ChannelsQuery
    {
        private readonly IMapper _mapper;

        public ChannelsQuery(IMapper mapper)
        {
            _mapper = mapper;
        }

        [UseDbContext(typeof(MediaContext))]
        public IQueryable<ChannelDto> GetChannels([Service] MediaContext context)
        {
            var channels = context.Channels.Select(c => _mapper.Map<ChannelDto>(c));

            return channels; 
        }
    }
}
