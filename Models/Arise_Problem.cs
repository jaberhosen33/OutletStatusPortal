using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OutletStatusPortal.Models
{
    public class Arise_Problem
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("NewOutletInfo")]
        public int Sl { get; set; }

        public NewOutletInfo Outlet { get; set; }

        public string ProblemType { get; set; }

        public string Description { get; set; }

        [Required]
        public string Status { get; set; } // dropdown: pending / inprogress / resolved

        [DataType(DataType.Date)]
        public DateTime Date { get; set; } = DateTime.Now;

        public string UpdateBy { get; set; }
    }
}
