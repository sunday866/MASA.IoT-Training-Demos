namespace MASA.IoT.Core.Contract.Mqtt
{
    /// <summary>
    /// 连接事件请求
    /// </summary>
    public class ConnectedEventRequest
    {
        /// <summary>
        /// 设备名称
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// 时间戳
        /// </summary>
        public long Timestamp { get; set; }

        /// <summary>
        /// 事件（连接/断开）
        /// </summary>
        public string Event { get; set; }
        /// <summary>
        /// 连接时间（断开事件中为0）
        /// </summary>
        public long Connected_at { get; set; }

        /// <summary>
        /// Client ID
        /// </summary>
        public string Clientid { get; set; }
    }
}
