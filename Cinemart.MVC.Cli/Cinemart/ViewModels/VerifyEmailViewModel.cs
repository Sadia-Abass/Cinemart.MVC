using System.ComponentModel.DataAnnotations;

namespace Cinemart.ViewModels
{
    public class VerifyEmailViewModel
    {
        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string Email { get; set; } = string.Empty;
    }
}
