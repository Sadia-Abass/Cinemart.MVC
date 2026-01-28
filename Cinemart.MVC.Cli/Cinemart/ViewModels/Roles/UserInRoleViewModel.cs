namespace Cinemart.ViewModels.Roles
{
    public class UserInRoleViewModel
    {
        public Guid Id { get; init; }
        public string? Email { get; init; }
        public string? Firstname { get; init; } 
        public string? Lastname { get; init; }
        public bool IsActive { get; init; }
    }
}
