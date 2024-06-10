using MediaAPI.Schema.DataLoaders;

namespace MediaAPI.Models
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        // Resolver method to use DataLoader
        public Task<List<ChannelDto>> GetChannels(
            ChannelsDataLoader dataLoader)
        {
            return dataLoader.LoadAsync(Id);
        }
    }
}
