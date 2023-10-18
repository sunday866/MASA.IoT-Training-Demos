using MASA.IoT.Core.Contract.Measurement;
using Newtonsoft.Json;

namespace MASA.IoT.Core.Contract.Mqtt
{
    public class SendRpcMessageRequest
    {
        /// <summary>
        /// 设备名称
        /// </summary>
        public string? DeviceName { get; set; }

        /// <summary>
        /// 产品ID
        /// </summary>

        public Guid ProductId { get; set; } = Guid.Parse("c85ef7e5-2e43-4bd2-a939-07fe5ea3f459");

        /// <summary>
        /// 消息体
        /// </summary>
        public string MessageData { get; set; }

        /// <summary>
        /// 超时时间默认5s
        /// </summary>
        public int Timeout { get; set; } = 5;
    }
}
