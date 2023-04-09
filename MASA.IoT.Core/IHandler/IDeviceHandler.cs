using MASA.IoT.WebApi.Contract;

namespace MASA.IoT.WebApi.IHandler
{
    public interface IDeviceHandler
    {
        Task<DeviceRegResponse> DeviceRegAsync(DeviceRegRequest request);
    }
}