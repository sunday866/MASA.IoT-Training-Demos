namespace MASA.IoT.Core.Contract.Mqtt
{
    public class RespondRpcMessageRequest
    {
        /// <summary>
        /// Topic
        /// </summary>
        public string Topic { get; set; }

        /// <summary>
        /// 消息体
        /// </summary>
        public string Payload { get; set; }

        /// <summary>
        /// 消息Id（来自EMQX）
        /// </summary>
        public string MessageId { get; set; }
    }
}
