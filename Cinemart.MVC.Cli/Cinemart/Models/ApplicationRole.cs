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

    public class ApplicationRole : IdentityRole<Guid>
    {
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public virtual ICollection<ApplicationUserRole>? UserRoles { get; set; }
        public virtual ICollection<ApplicationRoleClaim>? RoleClaims { get; set; }
    }
}
