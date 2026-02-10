using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Cinemart.Models
{
    public class ApplicationUser : IdentityUser<Guid>
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
        public DateTime? DOB { get; set; }

        public DateTime? LastLogin { get; init; }

        public string imageUrl { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public bool IsActive { get; set; }

        public ICollection<TicketSale>? TicketSale { get; set; }
        public virtual ICollection<ApplicationUserClaim>? Claims { get; set; }
        public virtual ICollection<ApplicationUserLogin>? Logins { get; set; }
        public virtual ICollection<ApplicationUserToken>? Tokens { get; set; }
        public virtual ICollection<ApplicationUserRole>? UserRoles { get; set; }
    }

    public class ApplicationUserRole : IdentityUserRole<Guid>
    {
        public virtual ApplicationUser? User { get; set; }
        public virtual ApplicationRole? Role { get; set; }
    }

    public class ApplicationUserClaim : IdentityUserClaim<Guid>
    {
        public virtual ApplicationUser? User { get; set; }
    }

    public class ApplicationUserLogin : IdentityUserLogin<Guid>
    {
        public virtual ApplicationUser? User { get; set; }
    }

    public class ApplicationRoleClaim : IdentityRoleClaim<Guid>
    {
        public virtual ApplicationRole? Role { get; set; }
    }

    public class ApplicationUserToken : IdentityUserToken<Guid>
    {
        public virtual ApplicationUser? User { get; set; }
    }
}
