using InfluxDB.Client.Core;
using MASA.IoT.Common;
namespace MASA.IoT.Core.Contract.Device
{

    public class RpcMessageLog : RpcMessageLogBase
    {
        public RpcMessageLog()
        {

        }

        /// <summary>
        /// 消息体
        /// </summary>
        [IsValue]
        public string? MessageData { get; set; }
    }
}
