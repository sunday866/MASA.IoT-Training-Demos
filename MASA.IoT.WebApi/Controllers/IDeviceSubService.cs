using MASA.IoT.Common;

namespace MASA.IoT.WebApi.Controllers
{
    public interface IDeviceSubService
    {
        Task BusinessMQOperation(PubSubOptions options);
    }
}
