using System.ComponentModel.DataAnnotations;

namespace OutletStatusPortal.Models
{
    public class AfterOutletSetup
    {
        [Key]
        public int Id { get; set; }

        [Required] public string OutletName { get; set; }
        [Required] public string OfficeITSetup { get; set; }
        public string CourierStatus { get; set; }
        public string NetworkVendor { get; set; }
        public string OutletITSetup { get; set; }
        public string LinkStatus { get; set; }
        public string SapId { get; set; }
        public string MailId { get; set; }
        public string PosId { get; set; }
        public string EPSLive { get; set; }

        public string? AssignedPersons { get; set; }
    }

}
