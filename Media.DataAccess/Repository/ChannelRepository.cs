using Media.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Media.DataAccess.Repository
{
    public class ChannelRepository : IRepository<Channel>
    {
        private readonly MediaContext _context;

        public MediaContext Context => _context;

        public ChannelRepository(MediaContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Channel>> GetAllAsync()
        {
            return await _context.Channels.ToListAsync();
        }

        public async Task<Channel> GetByIdAsync(int id)
        {
            return await _context.Channels.FindAsync(id);
        }

        public async Task AddAsync(Channel entity)
        {
            await _context.Channels.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task AddRangeAsync(IEnumerable<Channel> entities)
        {
            await _context.Channels.AddRangeAsync(entities);
            await _context.SaveChangesAsync();

        }

        public async Task UpdateAsync(Channel entity)
        {
            var existingEntity = _context.Channels.Local.FirstOrDefault(e => e.ChannelId == entity.ChannelId);

            if (existingEntity == null)
            {
                existingEntity = await _context.Channels.FindAsync(entity.ChannelId);
            }

            if (existingEntity != null)
            {
                _context.Entry(existingEntity).State = EntityState.Detached;
            }

            _context.Channels.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Channel entity)
        {
            var existingEntity = _context.Channels.Local.FirstOrDefault(e => e.ChannelId == entity.ChannelId);

            if (existingEntity == null)
            {
                existingEntity = await _context.Channels.FindAsync(entity.ChannelId);
            }

            if (existingEntity != null)
            {
                _context.Entry(existingEntity).CurrentValues.SetValues(entity);
                _context.Channels.Remove(existingEntity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
