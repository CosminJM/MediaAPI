namespace MediaAPI.Models
{
    public class ChannelForUpdateDto
    {
        public Guid ChannelId { get; set; }
        public string? ChannelIdentificator { get; set; }
        public string? Name { get; set; }
    }
}
