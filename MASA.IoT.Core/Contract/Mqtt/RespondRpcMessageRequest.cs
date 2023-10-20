namespace MASA.IoT.Core.Contract.Mqtt
{
    public class RespondRpcMessageRequest
    {
        /// <summary>
        /// Topic
        /// </summary>
        public required string Topic { get; set; }

        /// <summary>
        /// 消息体
        /// </summary>
        public required string Payload { get; set; }

        /// <summary>
        /// 消息Id（来自EMQX）
        /// </summary>
        public required string MessageId { get; set; }
    }
}
