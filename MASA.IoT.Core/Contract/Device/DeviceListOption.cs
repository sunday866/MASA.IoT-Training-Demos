using MASA.IoT.Core.Contract.Base;

namespace MASA.IoT.Core.Contract.Device
{
    public class DeviceListOption : PagingOptions
    {
        public Guid ProductId { get; set; }
    }
}
