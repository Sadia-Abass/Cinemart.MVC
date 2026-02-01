namespace Cinemart.ViewModels.Users
{
    public class UserDetailsViewModel
    {
        public Guid Id { get; init; }
        public string Email { get; init; } = string.Empty;
        public string UserName { get; init; } = string.Empty;
        public string FirstName { get; init; } = string.Empty;
        public string? LastName { get; init; }
        public DateTime? DOB { get; init; }
        public DateTime? LastLogin { get; init; }
        public bool IsActive { get; init; }
        public bool EmailConfirmed { get; init; }
        public string imageUrl { get; set; } = string.Empty;
        public DateTime? CreatedDate { get; init; }
        public DateTime? ModifiedDate { get; init; }
        public List<string> Roles { get; init; } = new();
    }
}
