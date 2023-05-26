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
    }
}
