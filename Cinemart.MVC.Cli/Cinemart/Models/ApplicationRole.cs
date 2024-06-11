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
        public Role RoleTitle { get; set; }
    }
}
