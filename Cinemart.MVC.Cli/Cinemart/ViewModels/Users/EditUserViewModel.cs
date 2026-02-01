using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Cinemart.ViewModels.Users
{
    public class EditUserViewModel : IValidatableObject
    {
        [Required(ErrorMessage = "Invalid user.")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters.")]
        [Display(Name = "First Name")]
        public string? FirstName { get; set; }

        [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters.")]
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Enter a valid email address.")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Active?")]
        public bool IsActive { get; set; }

        [Display(Name = "Email Confirmed?")]
        public bool EmailConfirmed { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public DateTime? DOB { get; set; }

        public string imageUrl { get; set; } = string.Empty;

        // Keep this posted via hidden input; adding [HiddenInput] is optional since your view already posts it.
        [Required(ErrorMessage = "Concurrency token is missing. Please reload and try again.")]
        [HiddenInput(DisplayValue = false)]
        public string? ConcurrencyStamp { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DOB.HasValue && DOB.Value.Date > DateTime.Today)
            {
                yield return new ValidationResult("Date of birth cannot be in the future.", new[] { nameof(DOB) });
            }
        }
    }
}
