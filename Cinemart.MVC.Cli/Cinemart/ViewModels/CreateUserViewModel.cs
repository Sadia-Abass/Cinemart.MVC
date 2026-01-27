using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Cinemart.ViewModels
{
    public class CreateUserViewModel
    {
        [StringLength(100)]
        [Display(Name = "First Name")]
        [Required(ErrorMessage = "Enter your First Name")]
        public string Firstname { get; set; } = string.Empty;

        [StringLength(100)]
        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Enter your Last Name")]
        public string Lastname { get; set; } = string.Empty;

        [Display(Name = "Date of Birth")]
        [Required(ErrorMessage = "Enter your Date of Birth")]
        public DateTime DOB { get; set; }

        public string imageUrl { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public bool IsActive { get; set; }
    }
}
