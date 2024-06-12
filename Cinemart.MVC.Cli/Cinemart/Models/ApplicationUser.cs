using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Cinemart.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
    
        [StringLength(100)]
        [Display(Name = "First Name")]
        [PersonalData]
        [Required(ErrorMessage = "Enter your First Name")]
        public string Firstname { get; set; } = string.Empty;
        [StringLength(100)]
        [Display(Name = "Last Name")]
        [PersonalData]
        [Required(ErrorMessage = "Enter your Last Name")]
        public string Lastname { get; set; } = string.Empty;
        [Display(Name = "Date of Birth")]
        [PersonalData]
        [Required(ErrorMessage = "Enter your Date of Birth")]
        public DateTime DOB { get; set; }

        public ICollection<TicketSale>? TicketSale { get; set; }
    }
}
