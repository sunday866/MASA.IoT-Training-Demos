using InfluxDB.Client.Core;
using MASA.IoT.Common;
namespace MASA.IoT.Core.Contract.Device
{

    public class RpcMessageLogAll : RpcMessageLogBase
    {
        public RpcMessageLogAll()
        {

        }

        /// <summary>
        /// 消息体(下发)
        /// </summary>

        public string? DownMessageData { get; set; }


        /// <summary>
        /// 接口最终返回
        /// </summary>

        public string? ApiLog { get; set; }
    }
}
