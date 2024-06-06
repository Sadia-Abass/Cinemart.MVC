using System.ComponentModel.DataAnnotations;

namespace Cinemart.Models
{
    public class Film
    {
        [Key]
        public int FilmId { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        [Required]
        public DateTime YearOfRelease { get; set; }
        [Required]
        public int Duration { get; set; }
        [Required]
        public string Certificate { get; set; } = string.Empty;
        [Required]
        public List<Genre> Genres { get; set; } = new List<Genre>();
        [Required]
        public string FilmImageUrl { get; set; } = string.Empty;
        public List<FilmShowing>? FilmShowing { get; set; }
    }
}
