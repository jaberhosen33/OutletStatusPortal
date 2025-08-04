using System.ComponentModel.DataAnnotations;

namespace OutletStatusPortal.Models
{
    public class UserFormViewModel
    {
        [Required]
        public string StafId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string PassWord { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("PassWord", ErrorMessage = "Password and Confirm Password do not match.")]
        public string ConfirmPassword { get; set; }

        [Phone]
        public string Phone { get; set; }

        [Required]
        public string Role { get; set; }
    }
}
