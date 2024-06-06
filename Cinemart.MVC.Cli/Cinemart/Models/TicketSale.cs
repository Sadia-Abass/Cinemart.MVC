using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinemart.Models
{
    public class TicketSale
    {
        [Key]
        public int TicketSaleId { get; set; }
        [Required]
        public DateOnly SalesDate { get; set; }
        [Required]
        public TimeOnly SalesTime { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public decimal TotalPrice { get; set; }
        [ForeignKey(nameof(User))]
        [Required]
        public int UserId { get; set; }
        public User? User { get; set; }
        [ForeignKey(nameof(FilmShowing))]
        [Required]
        public int FilmShowingId { get; set; }
        public FilmShowing? FilmShowing { get; set; }
    }
}
