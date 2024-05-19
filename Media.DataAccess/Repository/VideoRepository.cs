using Media.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Media.DataAccess.Repository
{
    public class VideoRepository : IRepository<Video>
    {
        private readonly MediaContext _context;
        public MediaContext Context => _context;

        public VideoRepository(MediaContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Video>> GetAllAsync()
        {
            return await _context.Videos.ToListAsync();
        }

        public async Task<Video> GetByIdAsync(int id)
        {
            return await _context.Videos.FindAsync(id);
        }

        public async Task AddAsync(Video entity)
        {
            await _context.Videos.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Video entity)
        {
            _context.Videos.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Video entity)
        {
            _context.Videos.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task AddRangeAsync(IEnumerable<Video> entities)
        {
            await _context.Videos.AddRangeAsync(entities);
            await _context.SaveChangesAsync();
        }
    }
}
