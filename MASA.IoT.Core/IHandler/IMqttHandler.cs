using MASA.IoT.Core.Contract.Mqtt;

namespace MASA.IoT.Core.IHandler
{
    public interface IMqttHandler
    {
        Task<AddDeviceResponse> DeviceRegAsync(string deviceName, string password);
        Task<EmqxBaseResponse> PublishToMqttAsync(PublishMessageRequest request);
    }
}