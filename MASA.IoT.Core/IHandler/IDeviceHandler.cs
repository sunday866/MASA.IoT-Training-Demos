using MASA.IoT.Core.Contract.Enum;
using MASA.IoT.WebApi.Contract;

namespace MASA.IoT.WebApi.IHandler
{
    public interface IDeviceHandler
    {
        Task<DeviceRegResponse> DeviceRegAsync(DeviceRegRequest request);

        Task UpdateDeviceOnlineStatusAsync(string deviceName, OnLineStates onlineStatus);
    }
}