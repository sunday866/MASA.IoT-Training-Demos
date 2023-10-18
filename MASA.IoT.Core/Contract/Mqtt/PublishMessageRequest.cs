using System.Text.Json.Serialization;

namespace MASA.IoT.Core.Contract.Mqtt
{
    //[DataContract]
    public class PublishMessageRequest
    {
        /// <summary>
        /// Topic
        /// </summary>
        [JsonPropertyName("topic")]
        public string Topic { get; set; }

        /// <summary>
        /// 消息体
        /// </summary>
        [JsonPropertyName("payload")] 
        public string Payload { get; set; }
    }
}
