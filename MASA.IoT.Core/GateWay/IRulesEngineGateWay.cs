using MASA.IoT.Common;
using Newtonsoft.Json.Linq;

namespace MASA.IoT.Core.GateWay
{
    public interface IRulesEngineGateWay
    {
        Task<bool> SendDataAsync(PubSubOptions pubSubOptions);
    }
}
