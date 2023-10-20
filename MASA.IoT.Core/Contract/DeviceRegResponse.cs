namespace MASA.IoT.WebApi.Contract
{
    public class DeviceRegResponse
    {
        public string DeviceName { get; set; }
        public string Password { get; set; }

        public bool Succeed { get; set; }

        public string ErrMsg { get; set; }
    }
}
