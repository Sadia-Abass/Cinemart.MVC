using Cinemart.ViewModels;
using Cinemart.ViewModels.Users;

namespace Cinemart.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserViewModel>> GetAll();
        Task<UserViewModel> GetUserByIdAsync(Guid id);
        Task<CreateUserViewModel> CreateUserAsync(CreateUserViewModel createUserViewModel);
        Task<EditUserViewModel> UpdateUserAsync(EditUserViewModel editUserViewModel);
        Task<UserViewModel> DeleteUserAsync(Guid id);
        Task<UserRolesViewModel> ManageRolesAsync(UserRolesViewModel userRoleViewModel);
    }
}
