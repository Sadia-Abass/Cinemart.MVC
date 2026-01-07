using System.ComponentModel.DataAnnotations;

namespace Cinemart.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Token { get; set; } 

        [Required(ErrorMessage = "New Password is required.")]
        [StringLength(34, ErrorMessage = "The {0} must be at least {1} characters long.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Confirm New Password is required.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm New Password")]
        [Compare("NewPassword", ErrorMessage = "The New Password and confirm password entered do not match.")]
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }
}
