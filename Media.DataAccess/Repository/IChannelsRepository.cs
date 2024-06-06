using Media.Domain;

namespace Media.DataAccess.Repository
{
    public interface IChannelsRepository
    {
        Task AddChannelWithUserAsync(Channel channel, string username);
        Task AddRangeAsync(IEnumerable<Channel> entities);
        Task<bool> ChannelForUserExistsAsync(string channelIdentificator, string username);
        void Delete(Channel channel);
        Task<(IEnumerable<Channel>, int)> GetAllByUserAsync(string username, int pageNumber, int pageSize, string search);
        Task<Channel> GetByIdAndUserAsync(Guid id, string username);
        Task<bool> SaveChangesAsync();
    }
}