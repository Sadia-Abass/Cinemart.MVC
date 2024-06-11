using System.ComponentModel.DataAnnotations;

namespace Cinemart.Models
{
    public class Film
    {
        [Key]
        public int FilmId { get; set; }
        [Required(ErrorMessage = "Enter Film Title")]
        [StringLength(100)]
        [Display(Name = "Film Title")]
        public string Title { get; set; } = string.Empty;
        [Required(ErrorMessage = "Enter Film Description")]
        [Display(Name = "Film Description")]
        [DataType(DataType.Text)]
        public string Description { get; set; } = string.Empty;
        [Required(ErrorMessage = "Enter Year of Release")]
        [Display(Name = "Year of Release")]
        [DataType(DataType.Date)]
        public DateTime YearOfRelease { get; set; }
        [Required(ErrorMessage = "Enter Film Duration")]
        [DataType(DataType.Duration)]
        public int Duration { get; set; }
        [Required(ErrorMessage = "Enter Certificate")]
        [DataType(DataType.Text)]
        public string Certificate { get; set; } = string.Empty;
        [Required(ErrorMessage = "Select a Genre")]
        public List<Genre> Genres { get; set; } = new List<Genre>();
        public string FilmImageUrl { get; set; } = string.Empty;
        public List<FilmShowing>? FilmShowing { get; set; }
    }
}
