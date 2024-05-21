using System.ComponentModel.DataAnnotations;

namespace MediaAPI.Models
{
    public class UserForRegistrationDto
    {
        [MaxLength(50)]
        public string? Username { get; set; }
        [MaxLength(100)]
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
