namespace Cinemart.ViewModels
{
    public class UserRolesViewModel
    {
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public List<UserRoleItem> Roles { get; set; } = new List<UserRoleItem>();
    }
}
