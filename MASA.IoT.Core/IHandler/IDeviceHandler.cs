using MASA.IoT.Core.Contract.Device;
using MASA.IoT.Core.Contract.Enum;
using MASA.IoT.WebApi.Contract;
using Masa.Utils.Models;
using MASA.IoT.Common;
using MASA.IoT.Core.Contract.Measurement;
using MASA.IoT.Core.Contract.Mqtt;

namespace MASA.IoT.Core.IHandler
{
    public interface IDeviceHandler
    {
        Task<DeviceRegResponse> DeviceRegAsync(DeviceRegRequest request);

        Task UpdateDeviceOnlineStatusAsync(string deviceName, OnLineStates onlineStatus);
        Task<PaginatedListBase<DeviceListViewModel>> GetDeviceListBaseAsync(DeviceListOption options);
        Task<bool> WriteMeasurementAsync<T>(PubSubOptions pubSubOptions);
        Task<bool> WriteTestDataAsync();
        Task<EChartsData> GetDeviceDataPointListAsync(GetDeviceDataPointListOption option);

        Task<RpcMessageResponse> WriteRPCMessageAsync(RpcMessageRequest request);
    }
}