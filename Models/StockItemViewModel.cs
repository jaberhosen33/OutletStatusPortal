using System.ComponentModel.DataAnnotations;

// Assuming StockTypeEnum is here

namespace OutletStatusPortal.Models
{
    public class StockItemViewModel : IValidatableObject
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Vendor Name is required.")]
        public string VendorName { get; set; }

        public string? OutletName { get; set; }

        [Required(ErrorMessage = "Stock Type is required.")]
        public StockTypeEnum StockType { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "POS quantity must be non-negative.")]
        public int Pos { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "OM quantity must be non-negative.")]
        public int Om { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Server quantity must be non-negative.")]
        public int Server { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Router quantity must be non-negative.")]
        public int Router { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Scanner quantity must be non-negative.")]
        public int Scanner { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "ICMO quantity must be non-negative.")]
        public int Icmo { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (StockType == StockTypeEnum.DirectStockForOutlet && string.IsNullOrWhiteSpace(OutletName))
            {
                yield return new ValidationResult("Outlet Name is required for Direct Stock For Outlet.", new[] { nameof(OutletName) });
            }
        }
    }
}
