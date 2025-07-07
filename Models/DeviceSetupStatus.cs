using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OutletStatusPortal.Models
{
    public class DeviceSetupStatus
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("NewOutletInfo")]
        public int Sl { get; set; }

        public NewOutletInfo Outlet { get; set; }

        [Required]
        public string DeviceType { get; set; } // dropdown

        public string WorkStatus { get; set; }

        public string WorkBy { get; set; }

        [DataType(DataType.Date)]
        public DateTime UpdateDate { get; set; } = DateTime.Now;
    }
}
