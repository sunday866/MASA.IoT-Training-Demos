using MASA.IoT.Common;

namespace MASA.IoT.Core.GateWay
{
    public interface IRulesEngineGateWay
    {
        Task<bool> SendDataAsync(PubSubOptions pubSubOptions);
    }
}
