using Media.Domain;

namespace Media.DataAccess.Repository
{
    public interface IChannelsRepository
    {
        Task AddAsync(Channel entity);
        Task AddChannelWithUserAsync(Channel channel, string username);
        Task AddRangeAsync(IEnumerable<Channel> entities);
        Task<bool> ChannelExistsAsync(string channelIdentificator);
        Task<bool> ChannelForUserExistsAsync(string channelIdentificator, string username);
        void Delete(Channel entity);
        Task<IEnumerable<Channel>> GetAllAsync();
        Task<(IEnumerable<Channel>,int)> GetAllByUserAsync(string username, int pageNumber, int pageSize);
        Task<Channel> GetByIdAndUserAsync(int id, string username);
        Task<Channel> GetByIdAsync(int id);
        Task<bool> SaveChangesAsync();
        Task UpdateAsync(Channel entity);
    }
}