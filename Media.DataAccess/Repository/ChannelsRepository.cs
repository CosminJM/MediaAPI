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

        public async Task<(IEnumerable<Channel>, int)> GetAllByUserAsync(string username, int pageNumber, int pageSize, string search)
        {
            var foundData = _context.Channels
                .Where(c => c.Users.Any(u => u.Username == username) && (string.IsNullOrEmpty(search)/*return true (all values for user) if search is empty */ || c.Name.Contains(search) || c.ChannelIdentificator.Contains(search)));
            var totalRecords = await foundData.CountAsync();

            var paginatedData = await foundData
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .OrderBy(c => c.Name)
                .ToListAsync();

            return (paginatedData, totalRecords);
        }

        public async Task<Channel> GetByIdAndUserAsync(int id, string username)
        {
            return await _context.Channels.Where(c => c.Users.Any(u => u.Username == username) && c.ChannelId == id).FirstOrDefaultAsync();
        }

        public async Task<bool> ChannelForUserExistsAsync(string channelIdentificator, string username)
        {
            return await _context.Channels.AnyAsync(c => c.Users.Any(u => u.Username == username) && c.ChannelIdentificator == channelIdentificator);
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
