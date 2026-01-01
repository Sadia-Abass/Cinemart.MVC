using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Cinemart.ViewModels
{
    public class RegisterViewModel
    {
        [StringLength(100)]
        [Display(Name = "First Name")]
        [Required(ErrorMessage = "Enter your First Name")]
        public string Firstname { get; set; } = string.Empty;

        [StringLength(100)]
        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Enter your Last Name")]
        public string Lastname { get; set; } = string.Empty;

        [EmailAddress]
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Enter your email")]
        public string Email { get; set; } = string.Empty;

        [StringLength(34, ErrorMessage = "The {0} must be at least {1} characters long.", MinimumLength = 8)]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Enter password")]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Enter confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirm password entered do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;


        [Display(Name = "Date of Birth")]
        [Required(ErrorMessage = "Enter your Date of Birth")]
        public DateTime DOB { get; set; }

        public string imageUrl { get; set; } = string.Empty;

        public string ReturnUrl { get; set; } = string.Empty;

    }
}
