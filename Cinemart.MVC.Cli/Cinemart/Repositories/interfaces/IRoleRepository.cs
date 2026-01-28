using Cinemart.ViewModels;
using Cinemart.ViewModels.Roles;
using Microsoft.AspNetCore.Identity;

namespace Cinemart.Repositories.interfaces
{
    public interface IRoleRepository
    {
        Task<PagedResult<RoleViewModel>> GetAllRolesAsync(RolefilterViewModel rolefilterViewModel);
        Task<(IdentityResult Result, Guid? RoleId)> CreateRoleAsync(CreateRoleViewModel createRoleViewModel);
        Task<EditRoleViewModel> GetRoleForEditAsync(Guid roleId);
        Task<IdentityResult> EditRoleAsync(EditRoleViewModel editRoleViewModel);
        Task<IdentityResult> DeleteRoleAsync(Guid roleId);
        Task<RoleDetailsViewModel?> GetRoleDetailsAsync(Guid roleId, int pageNumber, int pageSize);
    }
}
