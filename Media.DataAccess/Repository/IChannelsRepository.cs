using Media.Domain;

namespace Media.DataAccess.Repository
{
    public interface IChannelsRepository
    {
        Task AddAsync(Channel entity);
        Task AddRangeAsync(IEnumerable<Channel> entities);
        Task<bool> ChannelExistsAsync(string channelIdentificator);
        void Delete(Channel entity);
        Task<IEnumerable<Channel>> GetAllAsync();
        Task<Channel> GetByIdAsync(int id);
        Task<bool> SaveChangesAsync();
        Task UpdateAsync(Channel entity);
    }
}