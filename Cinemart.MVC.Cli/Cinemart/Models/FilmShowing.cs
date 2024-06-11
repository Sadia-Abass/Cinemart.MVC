using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinemart.Models
{
    public class FilmShowing
    {
        [Key]
        public int FilmShowingId { get; set; }
        [Required]
        [Display(Name = "Showing Date")]
        [DataType(DataType.Date)]
        public DateTime ShowDate { get; set; }
        [Required]
        [DataType(DataType.Time)]
        [Display(Name = "Start Time")]
        public DateTime StartTime { get; set; }
        [Required]
        [DataType(DataType.Time)]
        [Display(Name = "End Time")]
        public DateTime EndTime { get; set; }
        [Required]
        [Range(1, 100)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        [Display(Name = "Price")]
        public decimal Price { get; set; }
  
        public int FilmId { get; set; }
        [ForeignKey(nameof(FilmId))]
        [Required]
        public Film? Film { get; set; }
        public List<TicketSale>? TicketSale { get; set; }
    }
}
