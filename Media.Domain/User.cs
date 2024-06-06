using System;
using System.ComponentModel.DataAnnotations;

namespace Media.Domain
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        public byte[] PasswordHash { get; set; }

        [Required]
        public byte[] PasswordSalt { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedDate { get; set; }

        // Navigation property for channels owned by the user
        public ICollection<Channel> Channels { get; set; }

        // Navigation property for videos uploaded by the user
        public ICollection<Video> Videos { get; set; }
    }
}
