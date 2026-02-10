using Cinemart.ViewModels;
using Cinemart.ViewModels.Users;
using Microsoft.AspNetCore.Identity;

namespace Cinemart.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<PagedResult<UserViewModel>> GetAllUsersAsync(UserListFilterViewModel filter);
        Task<UserDetailsViewModel?> GetUserDetailsAsync(Guid id);
        Task<(IdentityResult Result, Guid? UserId)> CreateUserAsync(CreateUserViewModel createUserViewModel);
        Task<EditUserViewModel?> GetForEditAsync(Guid id);
        Task<IdentityResult> UpdateUserAsync(EditUserViewModel editUserViewModel);
        Task<IdentityResult> DeleteUserAsync(Guid id);
        Task<UserRolesEditViewModel?> GetRolesForEditAsync(Guid userId);
        Task<IdentityResult> UpdateRolesAsync(Guid userId, IEnumerable<Guid> selectedRoleIds);
    }
}
