using System.ComponentModel.DataAnnotations;

namespace OutletStatusPortal.Models
{
    public class Users
    {
            [Key]
            public string StafId { get; set; }

            [Required]
            public string Name { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string PassWord { get; set; }

            [Phone]
            public string Phone { get; set; }

            [Required]
            public string Role { get; set; }


            [DataType(DataType.Date)]
            public DateTime Date { get; set; } = DateTime.Now;



    }
}
