using Cinemart.Models;
using Cinemart.Repositories.Interfaces;
using Cinemart.Services.Interfaces;
using Cinemart.ViewModels;
using Cinemart.ViewModels.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Cinemart.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IFileUploaderService _fileUploaderService;

        public UserRepository(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IFileUploaderService fileUploaderService) 
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _fileUploaderService = fileUploaderService;
        }


        public Task<CreateUserViewModel> CreateUserAsync(CreateUserViewModel createUserViewModel)
        {
            throw new NotImplementedException();
        }

        public async Task<UserViewModel> DeleteUserAsync(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            var roles = await _userManager.GetRolesAsync(user);
            var deleteImage = await _fileUploaderService.DeleteFileAsync(user.imageUrl);

            var model = new UserViewModel
            {
                Id = user.Id,
                Email = user.Email ?? string.Empty,
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                DOB = user.DOB,
                imageUrl = user.imageUrl,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt,
                Roles = roles.ToList(),
            };

            return model;
        }

        public async Task<IEnumerable<UserViewModel>> GetAll()
        {
            var users = await _userManager.Users.ToListAsync();
            var userViewModels = new List<UserViewModel>();

            foreach (var user in users) 
            {
                var roles = await _userManager.GetRolesAsync(user);
                userViewModels.Add(new UserViewModel 
                {
                    Id = user.Id,
                    Email = user.Email ?? string.Empty,
                    Firstname = user.Firstname,
                    Lastname = user.Lastname,
                    DOB = user.DOB,
                    imageUrl = user.imageUrl,
                    CreatedAt = user.CreatedAt,
                    UpdatedAt = user.UpdatedAt,
                    Roles = roles.ToList(),
                });
            }

            return userViewModels;
        }

        public async Task<UserViewModel> GetUserByIdAsync(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            var roles = await _userManager.GetRolesAsync(user);
            var model = new UserViewModel
            {
                Id = user.Id,
                Email = user.Email ?? string.Empty,
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                DOB = user.DOB,
                imageUrl = user.imageUrl,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt,
                Roles = roles.ToList(),
            };

            return model;

        }

        public Task<UserRolesViewModel> ManageRolesAsync(UserRolesViewModel userRoleViewModel)
        {
            throw new NotImplementedException();
        }

        public Task<EditUserViewModel> UpdateUserAsync(EditUserViewModel editUserViewModel)
        {
            throw new NotImplementedException();
        }
    }
}
