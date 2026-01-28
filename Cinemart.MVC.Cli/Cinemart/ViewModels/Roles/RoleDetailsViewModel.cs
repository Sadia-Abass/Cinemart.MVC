using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cinemart.ViewModels.Roles
{
    public class RoleDetailsViewModel
    {
        public Guid Id { get; init; }
        public string? Name { get; init; }
        public string? Description { get; init; }
        public bool IsActive { get; init; }
        public DateTime? CreatedDate { get; init; }
        public DateTime ModifiedDate { get; init; }
        public PagedResult<UserInRoleViewModel> Users { get; init; } = new();
    }
}
