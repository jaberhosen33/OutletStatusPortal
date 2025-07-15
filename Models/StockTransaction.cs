using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OutletStatusPortal.Models
{
    public class StockTransaction
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("StockItem")]
        public int StockItemId { get; set; }
        public StockItem StockItem { get; set; }

        [Required]
        public OperationType OperationType { get; set; }

        [Required]
        public DateTime OperationDate { get; set; } = DateTime.Now;

        [Required]
        [MaxLength(100)]
        public string PerformedBy { get; set; }

        [MaxLength(100)]
        public string? OutletCode { get; set; }

        public int Pos { get; set; }
        public int Om { get; set; }
        public int Server { get; set; }
        public int Router { get; set; }
        public int Scanner { get; set; }
        public int Icmo { get; set; }
        public DateTime UpdateDate { get; set; } = DateTime.Now;
    }

}
