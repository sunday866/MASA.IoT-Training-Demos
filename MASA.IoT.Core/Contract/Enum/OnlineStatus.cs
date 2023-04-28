using System.ComponentModel;

namespace MASA.IoT.Core.Contract.Enum
{
    public enum OnLineStates
    {
        [Description("离线")]
        OffLine,
        [Description("在线")]
        OnLine,
        [Description("未配网")]
        WifiNotConfigured,
    }
}
