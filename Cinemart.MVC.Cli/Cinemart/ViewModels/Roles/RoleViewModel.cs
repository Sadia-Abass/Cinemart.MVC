using System.ComponentModel.DataAnnotations;

namespace Cinemart.ViewModels.Roles
{
    public class RoleViewModel
    {
        public Guid Id { get; init; }

        [Required]
        [Display(Name = "Role Name")]
        public string Name { get; init; } = string.Empty;

        [Display(Name = "Role Description")]
        public string Description { get; init; } = string.Empty;
        public bool IsActive { get; init; }
        public DateTime? CreatedDate { get; init; }
    }
}
