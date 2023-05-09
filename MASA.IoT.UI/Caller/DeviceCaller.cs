using Masa.Contrib.Service.Caller.DaprClient;
using Masa.Utils.Models;
using MASA.IoT.Core.Contract.Device;

namespace MASA.IoT.UI.Caller
{
    public class DeviceCaller : DaprCallerBase
    {
        private const string BASE_API = "/api/device";
        protected override string AppId { get; set; } = "MASA-IoT-Core";

        public Task<PaginatedListBase<DeviceListViewModel>?> DeviceListAsync(DeviceListOption option)
        {
            return Caller.PostAsync<PaginatedListBase<DeviceListViewModel>>($"{BASE_API}/DeviceList", option);
        }

    }
}
