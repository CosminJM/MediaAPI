namespace MediaAPI.Models
{
    public class RefreshToken
    {
        public string? Token { get; set; }
        public string? Username { get; set; }
        public Guid UserId { get; set; }
    }
}
