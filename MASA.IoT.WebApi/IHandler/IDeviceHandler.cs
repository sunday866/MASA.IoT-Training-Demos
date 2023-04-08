using MASA.IoT.WebApi.Contract;

namespace MASA.IoT.WebApi.IHandler
{
    public interface IDeviceHandler
    {
        Task<bool> DeviceRegAsync(DeviceRegRequest request);
    }
}