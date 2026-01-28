using System.ComponentModel.DataAnnotations;

namespace Cinemart.ViewModels.Roles
{
    public class EditRoleViewModel
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        [Display(Name = "Role Name")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Role Description")]
        public string Discription { get; set; } = string.Empty;

        [Display(Name = "Active?")]
        public bool IsActive { get; set; } = true;
        public string? ConcurrencyStamp { get; set; }
    }
}
