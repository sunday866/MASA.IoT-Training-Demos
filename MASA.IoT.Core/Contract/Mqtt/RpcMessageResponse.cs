namespace MASA.IoT.Core.Contract.Mqtt
{
    public class RpcMessageResponse
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 设备返回内容
        /// </summary>
        public string? DeviceResponse { get; set; }

        /// <summary>
        /// 产品ID
        /// </summary>

        public string? ErrorMessage { get; set; }

        /// <summary>
        /// 请求ID
        /// </summary>
        public Guid RequestId { get; set; }


        /// <summary>
        /// 消息ID（设备）
        /// </summary>
        public string MessageId { get; set; }
    }
}
