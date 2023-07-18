using Dapr;
using MASA.IoT.Common;
using MASA.IoT.Core.GateWay;
using MASA.IoT.Core.Handler;
using MASA.IoT.Core.IHandler;
using MASA.IoT.WebApi.Handler;
using MASA.IoT.WebApi.IHandler;
using Microsoft.AspNetCore.Mvc;

namespace MASA.IoT.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceMqttController : ControllerBase
    {
        private IDeviceHandler _deviceHandler;
        private IRulesEngineGateWay _rulesEngineGateWay;

        public DeviceMqttController(IDeviceHandler deviceHandler, IRulesEngineGateWay rulesEngineGateWay)
        {
            _deviceHandler = deviceHandler;
            _rulesEngineGateWay = rulesEngineGateWay;
        }

        [Topic("pubsub", "DeviceMessage")]
        [HttpPost("DeviceMessage")]
        public async Task DeviceMessageAsync([FromBody] PubSubOptions pubSubOptions)
        {
            await _rulesEngineGateWay.SendDataAsync(pubSubOptions);
            await _deviceHandler.WriteMeasurementAsync<PubSubOptions>(pubSubOptions);
            Console.WriteLine($"Subscriber received, DeviceName:{pubSubOptions.DeviceName},Msg:{pubSubOptions.Msg}");
        }

        [HttpPost("WriteTestData")]
        public async Task WriteTestDataAsync()
        {
            await _deviceHandler.WriteTestDataAsync();
            //Console.WriteLine($"Subscriber received, DeviceName:{pubSubOptions.DeviceName},Msg:{pubSubOptions.Msg}");
        }
    }
}
