namespace Media.Domain
{
    public class Channel
    {
        public Channel()
        {
            Videos = new List<Video>();
            Users = new List<User>();
        }

        public int ChannelId { get; set; }
        public string ChannelIdentificator { get; set; }
        public string Name { get; set; }
        public ICollection<Video> Videos { get; set; }
        public ICollection<User> Users { get; set; }
    }
}