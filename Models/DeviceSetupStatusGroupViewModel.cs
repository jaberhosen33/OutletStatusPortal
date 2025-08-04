namespace OutletStatusPortal.Models
{

    public class DeviceSetupStatusGroupViewModel
    {
        public BeforeOutletSetUp Outlet { get; set; }
        public List<DeviceSetupStatus> Devices { get; set; } = new List<DeviceSetupStatus>();
    }
}
