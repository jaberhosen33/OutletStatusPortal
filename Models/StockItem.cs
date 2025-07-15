using System.ComponentModel.DataAnnotations;

namespace OutletStatusPortal.Models
{
    public class StockItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string VendorName { get; set; }

        public string? OutletName { get; set; } // Only if DirectStockForOutlet

        [Required]
        public StockTypeEnum StockType { get; set; }

        // Quantity fields
        [Range(0, int.MaxValue)] public int Pos { get; set; }
        [Range(0, int.MaxValue)] public int Om { get; set; }
        [Range(0, int.MaxValue)] public int Server { get; set; }
        [Range(0, int.MaxValue)] public int Router { get; set; }
        [Range(0, int.MaxValue)] public int Scanner { get; set; }
        [Range(0, int.MaxValue)] public int Icmo { get; set; }

        public ICollection<StockTransaction> Transactions { get; set; }
    }

}
