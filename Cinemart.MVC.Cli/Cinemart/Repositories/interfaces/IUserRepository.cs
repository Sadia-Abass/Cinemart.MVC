using Cinemart.ViewModels;

namespace Cinemart.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserViewModel>> GetAll();
        Task<UserViewModel> GetUserByIdAsync(int id);
        Task<CreateUserViewModel> CreateUserAsync(CreateUserViewModel createUserViewModel);
        Task<EditUserViewModel> UpdateUserAsync(EditUserViewModel editUserViewModel);
        Task DeleteUserAsync(int id);
    }
}
