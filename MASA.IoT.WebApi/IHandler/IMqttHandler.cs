using MASA.IoT.WebApi.Contract;
using MASA.IoT.WebApi.Contract.Mqtt;

namespace MASA.IoT.WebApi.IHandler
{
    public interface IMqttHandler
    {
        Task<AddDeviceResponse> DeviceRegAsync(string DeviceName);
    }
}