using MASA.IoT.WebApi.Contract;
using MASA.IoT.WebApi.IHandler;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Text;
using MASA.IoT.Core.Contract;
using MASA.IoT.Core.Contract.Enum;
using MASA.IoT.Core.Contract.Mqtt;
using MASA.IoT.Core.Contract.Device;
using MASA.IoT.Core.IHandler;
using Masa.Utils.Models;

namespace MASA.IoT.WebApi.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private readonly IDeviceHandler _deviceHandler;

        public DeviceController(IDeviceHandler deviceHandler)
        {
            _deviceHandler = deviceHandler;
        }

        [HttpPost]

        public async Task<DeviceRegResponse> DeviceRegAsync(DeviceRegRequest request)
        {
            return await _deviceHandler.DeviceRegAsync(request);
        }

        /// <summary>
        /// 连接、断开事件
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task ConnectedEventAsync([FromBody] ConnectedEventRequest request)
        {
            var onlineStatus = request.Event switch
            {
                "client.connected" => OnLineStates.OnLine,
                _ => OnLineStates.OffLine
            };

            await _deviceHandler.UpdateDeviceOnlineStatusAsync(request.Username, onlineStatus);
        }

        /// <summary>
        /// 按照产品Id分页查询设备列表
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PaginatedListBase<DeviceListViewModel>> DeviceListAsync([FromBody] DeviceListOption options)
        {
           return await _deviceHandler.GetDeviceListBaseAsync(options);
        }

        [HttpPost]

        public async Task<EChartsData> GetDeviceDataPointListAsync([FromBody] GetDeviceDataPointListOption option)
        {
            return await _deviceHandler.GetDeviceDataPointListAsync(option);
        }


    }
}
