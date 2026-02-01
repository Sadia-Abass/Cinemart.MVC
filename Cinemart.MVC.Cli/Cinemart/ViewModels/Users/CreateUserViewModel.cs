using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Cinemart.ViewModels.Users
{
    public class CreateUserViewModel : IValidatableObject
    {
        [StringLength(100)]
        [Display(Name = "First Name")]
        [Required(ErrorMessage = "Enter your First Name")]
        public string Firstname { get; set; } = string.Empty;

        [StringLength(100)]
        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Enter your Last Name")]
        public string Lastname { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        [Required(ErrorMessage = "Enter your Date of Birth")]
        public DateTime? DOB { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Enter a valid email address.")]
        [Remote(action: "IsEmailAvailable", controller: "RemoteValidation", ErrorMessage = "This email is already registered.")]
        [Display(Name = "Email")]
        public string Email { get; set; } = null!;
       
        [Display(Name = "Active?")]
        public bool IsActive { get; set; }

        [Display(Name = "Mark Email Confirmed?")]
        public bool MarkEmailConfirmed { get; set; } = true;

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least {2} characters long.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Please confirm the password.")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; } = null!;

        [Display(Name = "Upload Picture")]
        public string imageUrl { get; set; } = string.Empty;
      
        // Model-level validations that are easier to express in code than attributes
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(DOB.HasValue && DOB.Value.Date > DateTime.Today)
            {
                yield return new ValidationResult("Date of birth cannot be in the future.", new[] {nameof(DOB)});
            }
        }
    }
}
