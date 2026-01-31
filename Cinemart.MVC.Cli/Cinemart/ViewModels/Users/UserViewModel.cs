using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Cinemart.ViewModels.Users
{
    public class UserViewModel
    {
        public Guid Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string UserName { get; set; } = string.Empty;

        [Display(Name = "First Name")]
        public string Firstname { get; set; } = string.Empty;

        [Display(Name = "Last Name")]
        public string Lastname { get; set; } = string.Empty;

        [Display(Name = "Date of Birth")]
        public DateTime DOB { get; set; }

        [Display(Name = "Picture")]
        public string imageUrl { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        public bool EmailConfirmed { get; set; }

        [Display(Name = "Date Joined")]
        public DateTime? CreatedAt { get; set; }

        //[Display(Name = "Last updated profile")]
        //public DateTime UpdatedAt { get; set; }

        //public List<string> Roles { get; set; } = new List<string>();
    }
}
