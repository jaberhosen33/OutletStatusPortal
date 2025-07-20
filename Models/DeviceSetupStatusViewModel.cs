using Microsoft.AspNetCore.Mvc.Rendering;

namespace OutletStatusPortal.Models
{
    public class DeviceSetupStatusFormViewModel
    {
        public int SelectedBeforeOutletSetUpSl { get; set; }

        public List<SelectListItem> BeforeOutletSetUpList { get; set; }

        public List<DeviceSetupStatus> DeviceStatuses { get; set; } = new List<DeviceSetupStatus>();
    }

}
