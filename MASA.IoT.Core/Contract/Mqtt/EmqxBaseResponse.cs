namespace MASA.IoT.Core.Contract.Mqtt
{
    public class EmqxBaseResponse
    {
        /// <summary>
        /// 全局唯一的一个消息 ID，方便用于关联和追踪
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// MQTT 消息发布的错误码，这些错误码也是 MQTT 规范中 PUBACK 消息可能携带的错误码
        /// </summary>
        public int reason_code { get; set; }

        public string Code { get; set; }

        /// <summary>
        /// 失败的详细原因
        /// </summary>
        public string Message { get; set; }
    }

}
