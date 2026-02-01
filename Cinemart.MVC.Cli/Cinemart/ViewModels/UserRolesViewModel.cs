using Cinemart.ViewModels.Users;

namespace Cinemart.ViewModels
{
    public class UserRolesViewModel
    {
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public List<UserRolesEditViewModel> Roles { get; set; } = new List<UserRolesEditViewModel>();
    }
}
