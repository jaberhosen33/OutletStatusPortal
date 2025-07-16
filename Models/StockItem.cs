using System.ComponentModel.DataAnnotations;

namespace OutletStatusPortal.Models
{
    public class StockItem : IValidatableObject
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string VendorName { get; set; }

        public string? OutletName { get; set; }

        [Required]
        public StockTypeEnum StockType { get; set; }

        [Range(0, int.MaxValue)] public int Pos { get; set; }
        [Range(0, int.MaxValue)] public int Om { get; set; }
        [Range(0, int.MaxValue)] public int Server { get; set; }
        [Range(0, int.MaxValue)] public int Router { get; set; }
        [Range(0, int.MaxValue)] public int Scanner { get; set; }
        [Range(0, int.MaxValue)] public int Icmo { get; set; }

        public ICollection<StockTransaction>? Transactions { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (StockType == StockTypeEnum.DirectStockForOutlet && string.IsNullOrWhiteSpace(OutletName))
            {
                yield return new ValidationResult("Outlet Name is required for Direct Stock For Outlet.", new[] { nameof(OutletName) });
            }
        }
    }
}
