namespace MASA.IoT.Core.Contract.Device
{
    public class DeviceListViewModel
    {
        /// <summary>
        /// 设备ID
        /// </summary>
        public Guid Id { get; set; }


        /// <summary>
        /// 设备名称
        /// </summary>
        public string? DeviceName { get; set; }
    }
}
