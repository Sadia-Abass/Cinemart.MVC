using Microsoft.AspNetCore.Identity;

namespace Cinemart.Models
{
    public enum Role
    {
        Admin,
        Manager,
        Employee,
        Member
    }

    public class ApplicationRole : IdentityRole<int>
    {
        public string Description { get; set; } = string.Empty;
        public virtual ICollection<ApplicationUserRole>? UserRoles { get; set; }
        public virtual ICollection<ApplicationRoleClaim>? RoleClaims { get; set; }
    }
}
