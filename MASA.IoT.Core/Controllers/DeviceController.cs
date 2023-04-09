using MASA.IoT.WebApi.Contract;
using MASA.IoT.WebApi.IHandler;
using Microsoft.AspNetCore.Mvc;

namespace MASA.IoT.WebApi.Controllers
{
    [Route("api/[controller]")]
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
    }
}
