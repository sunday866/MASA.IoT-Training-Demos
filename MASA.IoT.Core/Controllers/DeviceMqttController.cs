using Dapr;
using MASA.IoT.Common;
using MASA.IoT.Core.Handler;
using MASA.IoT.Core.IHandler;
using MASA.IoT.WebApi.IHandler;
using Microsoft.AspNetCore.Mvc;

namespace MASA.IoT.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceMqttController : ControllerBase
    {
        private IDeviceHandler _deviceHandler;

        public DeviceMqttController(IDeviceHandler deviceHandler)
        {
            _deviceHandler = deviceHandler;
        }

        [Topic("pubsub", "DeviceMessage")]
        [HttpPost("DeviceMessage")]
        public async Task DeviceMessageAsync([FromBody] PubSubOptions pubSubOptions)
        {

            await _deviceHandler.WriteMeasurementAsync<PubSubOptions>(pubSubOptions);
            Console.WriteLine($"Subscriber received, DeviceName:{pubSubOptions.DeviceName},Msg:{pubSubOptions.Msg}");
        }
    }
}
