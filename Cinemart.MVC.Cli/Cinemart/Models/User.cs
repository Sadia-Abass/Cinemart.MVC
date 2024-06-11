using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Cinemart.Models
{
    public class User : IdentityUser
    {
    
        [StringLength(100)]
        [Display(Name = "First Name")]
        [PersonalData]
        [Required]
        public string Firstname { get; set; } = string.Empty;
        [StringLength(100)]
        [Display(Name = "Last Name")]
        [PersonalData]
        [Required]
        public string Lastname { get; set; } = string.Empty;
        [StringLength(100)]
        [Display(Name = "Date of Birth")]
        [PersonalData]
        [Required]
        public DateTime DOB { get; set; }

    }
}
