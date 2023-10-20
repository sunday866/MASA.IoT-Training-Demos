using InfluxDB.Client.Core;
using MASA.IoT.Common;
using MASA.IoT.Core.Contract.Enum;
using Newtonsoft.Json;

namespace MASA.IoT.Core.Contract.Device
{

    public class RpcMessageLogBase
    {
       public RpcMessageLogBase()
        {

        }
        /// <summary>
        /// 设备名称
        /// </summary>
        public string? DeviceName { get; set; }

        /// <summary>
        /// 产品ID
        /// </summary>

        public Guid ProductId { get; set; }

        /// <summary>
        /// 消息类型
        /// </summary>
        public MessageType MessageType { get; set; }

        /// <summary>
        /// 请求ID
        /// </summary>
        public Guid RequestId { get; set; }

        /// <summary>
        /// 消息ID
        /// </summary>
        public string? MessageId { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        [IsTs]
        public DateTime? DateTime { get; set; }
    }
}
