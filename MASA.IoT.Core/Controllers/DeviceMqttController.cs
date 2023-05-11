using Dapr;
using MASA.IoT.Common;
using Microsoft.AspNetCore.Mvc;

namespace MASA.IoT.Core.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class DeviceMqttController : ControllerBase
    {
        [Topic("pubsub", "DeviceMessage")]
        [HttpPost("DeviceMessage")]
        public void DeviceMessageAsync([FromBody] PubSubOptions testStrData)
        {
            Console.WriteLine("Subscriber received : " + testStrData);
        }
    }
}
