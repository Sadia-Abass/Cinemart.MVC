using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinemart.Models
{
    public class TicketSale
    {
        [Key]
        public int TicketSaleId { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Sales Date")]
        public DateTime SalesDate { get; set; }
        [Required]
        [DataType(DataType.Time)]
        [Display(Name = "Sales Time")]
        public DateTime SalesTime { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        [Display(Name = "Total Price")]
        [Range(1, 100)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalPrice { get; set; }
      
        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public ApplicationUser? User { get; set; }
     
        public int FilmShowingId { get; set; }
        [ForeignKey(nameof(FilmShowingId))]
        public FilmShowing? FilmShowing { get; set; }
    }
}
