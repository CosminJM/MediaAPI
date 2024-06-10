using AutoMapper;
using Media.DataAccess;
using Media.DataAccess.Repository;
using MediaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace MediaAPI.Schema.DataLoaders
{
    public class ChannelsDataLoader : BatchDataLoader<Guid, List<ChannelDto>>
    {
        private readonly MediaContext _context;
        private readonly IMapper _mapper;

        public ChannelsDataLoader(IDbContextFactory<MediaContext> context,IMapper mapper, IBatchScheduler batchScheduler, DataLoaderOptions? options = null) : base(batchScheduler, options)
        {
            _context = context.CreateDbContext();
            _mapper = mapper;
        }

        protected override async Task<IReadOnlyDictionary<Guid, List<ChannelDto>>> LoadBatchAsync(IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
        {
            var channels = await _context.Users.Where(u => keys.Contains(u.Id))
                .Include(u => u.Channels)
                .ToDictionaryAsync(u => u.Id, u =>  _mapper.Map<List<ChannelDto>>(u.Channels.ToList()), cancellationToken);

            return channels;
        }
    }
}
