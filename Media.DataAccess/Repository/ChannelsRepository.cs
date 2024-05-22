using Media.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Media.DataAccess.Repository
{
    public class ChannelsRepository : IChannelsRepository
    {
        private MediaContext _context;

        public ChannelsRepository(MediaContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Channel>> GetAllAsync()
        {
            return await _context.Channels.ToListAsync();
        }

        public async Task<IEnumerable<Channel>> GetAllByUserAsync(string username)
        {
            return await _context.Channels.Where(c => c.Users.Any(u => u.Username == username)).ToListAsync();
        }

        public async Task<Channel> GetByIdAsync(int id)
        {
            return await _context.Channels.FindAsync(id);
        }

        public async Task<Channel> GetByIdAndUserAsync(int id, string username)
        {
            return await _context.Channels.Where(c => c.Users.Any(u => u.Username == username) && c.ChannelId == id).FirstOrDefaultAsync();
        }

        public async Task<bool> ChannelExistsAsync(string channelIdentificator)
        {
            return await _context.Channels.AnyAsync(c => c.ChannelIdentificator == channelIdentificator);
        }

        public async Task<bool> ChannelForUserExistsAsync(string channelIdentificator,string username)
        {
            return await _context.Channels.AnyAsync(c =>c.Users.Any(u => u.Username == username) && c.ChannelIdentificator == channelIdentificator);
        }

        public async Task AddAsync(Channel entity)
        {
            if (await ChannelExistsAsync(entity.ChannelIdentificator)) return;
            await _context.Channels.AddAsync(entity);
        }

        public async Task AddChannelWithUserAsync(Channel channel, string username)
        {
            await _context.Channels.AddAsync(channel);
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null) return;
            channel.Users.Add(user);
        }

        public async Task AddRangeAsync(IEnumerable<Channel> entities)
        {
            await _context.Channels.AddRangeAsync(entities);
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
        }

        //public async Task DeleteAsync(Channel entity)
        //{
        //    var existingEntity = _context.Channels.Local.FirstOrDefault(e => e.ChannelId == entity.ChannelId);

        //    if (existingEntity == null)
        //    {
        //        existingEntity = await _context.Channels.FindAsync(entity.ChannelId);
        //    }

        //    if (existingEntity != null)
        //    {
        //        _context.Entry(existingEntity).CurrentValues.SetValues(entity);
        //        _context.Channels.Remove(existingEntity);
        //    }
        //}

        public void Delete(Channel channel)
        {
            _context.Channels.Remove(channel);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }
    }
}
