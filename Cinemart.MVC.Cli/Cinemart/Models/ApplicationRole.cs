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
    }
}
