using System.ComponentModel.DataAnnotations;

namespace Cinemart.ViewModels.Roles
{
    public class CreateRoleViewModel
    {
        [Required(ErrorMessage = "Name is required.")]
        [Display(Name = "Role Name")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Role Description")]
        public string Description { get; set; } = string.Empty;

        [Display(Name = "Active?")]
        public bool IsActive { get; set; } = true;
    }
}
