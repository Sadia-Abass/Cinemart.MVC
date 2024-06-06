using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinemart.Models
{
    public class FilmShowing
    {
        [Key]
        public int FilmShowingId { get; set; }
        [Required]
        public DateOnly ShowDate { get; set; }
        [Required]
        public TimeOnly StartTime { get; set; }
        [Required]
        public TimeOnly EndTime { get; set; }
        [Required]
        public decimal Price { get; set; }
        [ForeignKey(nameof(Film))]
        [Required]
        public int FilmId { get; set; }
        public Film? Film { get; set; }
        public List<TicketSale>? TicketSale { get; set; }
    }
}
