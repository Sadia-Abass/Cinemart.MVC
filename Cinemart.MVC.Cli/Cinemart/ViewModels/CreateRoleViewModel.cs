using System.ComponentModel.DataAnnotations;

namespace Cinemart.ViewModels
{
    public class CreateRoleViewModel
    {
        [Required]
        [Display(Name = "Role Name")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Role Description")]
        public string Discription { get; set; } = string.Empty;
    }
}
