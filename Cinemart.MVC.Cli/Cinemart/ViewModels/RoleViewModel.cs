using System.ComponentModel.DataAnnotations;

namespace Cinemart.ViewModels
{
    public class RoleViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Role Name")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Role Description")]
        public string Discription { get; set; } = string.Empty;

        [Display(Name = "User Count")]
        public string UserCount { get; set; } = string.Empty;
    }
}
