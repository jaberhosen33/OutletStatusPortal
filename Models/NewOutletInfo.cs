using System.ComponentModel.DataAnnotations;

namespace OutletStatusPortal.Models
{
    public class NewOutletInfo
    {
        [Key]
        public int Sl { get; set; }

        public string? OutletCode { get; set; }

        [Required]
        public string OutletName { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public string OfficeITSetup { get; set; } // dropdown

        [Required]
        public string CouriarStatus { get; set; } // dropdown

        [Required]
        public string Vendor { get; set; }

        [Required]
        public string OutletITSetup { get; set; } // dropdown

        [Required]
        public string LinkStatus { get; set; } // dropdown

        [Required]
        public string SAPID { get; set; } // dropdown

        [Required]
        public string MailID { get; set; } // dropdown

        [Required]
        public string POSID { get; set; } // dropdown

        [Required]
        public string EPSLive { get; set; } // dropdown

        public string? AssignPersons { get; set; }

        [DataType(DataType.Date)]
        public DateTime UpdateDate { get; set; } = DateTime.Now;
    }
}
