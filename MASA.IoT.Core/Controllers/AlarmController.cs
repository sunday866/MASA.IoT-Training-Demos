using MASA.IoT.Common;
using MASA.IoT.WebApi.Contract;
using Microsoft.AspNetCore.Mvc;

namespace MASA.IoT.WebApi.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class AlarmController : ControllerBase
    {
        [HttpPost]

        public string TestAlarm(DeviceAlarmRequest request)
        {
            Console.WriteLine($"DeviceName:{request.DeviceName},AlarmMsg:{request.AlarmMsg}");
            return "告警通知成功";
        }

        [HttpPost]
        public async Task<bool> TestAsync([FromBody] PubSubOptions pubSubOptions)
        {

            Console.WriteLine(pubSubOptions.Msg);
           await Task.Delay(2000);
            
            return true;
            //await _deviceHandler.WriteMeasurementAsync<PubSubOptions>(pubSubOptions);
            //            Console.WriteLine($"Subscriber received, DeviceName:{pubSubOptions.DeviceName},Msg:{pubSubOptions.Msg}");
        }
    }
}
