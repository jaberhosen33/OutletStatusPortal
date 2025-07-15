using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OutletStatusPortal.Models
{
    public class BeforeOutletSetUp
    {
        [Key]
        public int Sl { get; set; }

       
        public string OutletCode { get; set; }
        public string Location { get; set; }

        [Required]
        public StockTypeEnum StockType { get; set; }

        [Required]
        [ForeignKey("StockItem")]
        public int StockItemId { get; set; }
        public StockItem StockItem { get; set; }

        // Quantities used
        public int Pos { get; set; }
        public int Om { get; set; }
        public int Server { get; set; }
        public int Router { get; set; }
        public int Scanner { get; set; }
        public int Icmo { get; set; }

        public ICollection<DeviceSetupStatus> DeviceStatuses { get; set; }
    }

}
