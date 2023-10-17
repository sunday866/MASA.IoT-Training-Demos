namespace MASA.IoT.Core.Contract.Mqtt
{
    public class RpcMessageResponse
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 产品ID
        /// </summary>

        public string? ErrorMessage { get; set; }

        /// <summary>
        /// 请求ID
        /// </summary>
        public Guid RequestId { get; set; }
    }
}
