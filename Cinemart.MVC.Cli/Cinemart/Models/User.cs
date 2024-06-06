using System.ComponentModel.DataAnnotations;

namespace Cinemart.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        public string Firstname { get; set; } = string.Empty;
        [Required]
        public string Lastname { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string EmailConfirmed { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        [Required]
        public string PasswordConfirmed { get; set; } = string.Empty;
        [Required]
        public string PhoneNumber { get; set; } = string.Empty;
        [Required]
        public string Role { get; set; } = string.Empty;
        public List<TicketSale>? TicketSale { get; set; }
    }
}
