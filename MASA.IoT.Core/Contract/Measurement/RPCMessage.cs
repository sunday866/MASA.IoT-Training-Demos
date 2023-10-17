using InfluxDB.Client.Core;
using Newtonsoft.Json;

namespace MASA.IoT.Core.Contract.Measurement
{
    [InfluxDB.Client.Core.Measurement("RPCMessage")]
    public class RPCMessage
    {
        /// <summary>
        /// 设备名称
        /// </summary>
        [Column("DeviceName", IsTag = true)] public string? DeviceName { get; set; }

        /// <summary>
        /// 产品ID
        /// </summary>
        [Column("ProductId", IsTag = true)]
        public Guid ProductId { get; set; } = Guid.Parse("c85ef7e5-2e43-4bd2-a939-07fe5ea3f459");

        /// <summary>
        /// 消息类型
        /// </summary>
        [Column("MessageType", IsTag = true)] public MessageType MessageType { get; set; }

        /// <summary>
        /// 请求ID
        /// </summary>
        [Column("RequestId", IsTag = true)] public Guid RequestId { get; set; }

        /// <summary>
        /// 消息ID
        /// </summary>
        [Column("MessageId", IsTag = true)] public Guid MessageId { get; set; }

        /// <summary>
        /// 消息体
        /// </summary>
        [Column("MessageData")] public string? MessageData { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        [JsonProperty(propertyName: "Ts")]
        [Column(IsTimestamp = true)] public long Timestamp { get; set; }
    }

    public enum MessageType
    {
        Up, //回复
        Down, //下发
        Other //其他
    }
}
