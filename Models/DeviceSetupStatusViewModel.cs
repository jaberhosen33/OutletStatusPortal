using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace OutletStatusPortal.Models
{
    public class DeviceSetupStatusFormViewModel
    {
        [Required(ErrorMessage = "Please select Before Outlet Setup.")]
        public int SelectedBeforeOutletSetUpSl { get; set; }
        [ValidateNever]
        public List<SelectListItem> BeforeOutletSetUpList { get; set; }

        public List<DeviceSetupStatus> DeviceStatuses { get; set; } = new List<DeviceSetupStatus>();
    }


}
