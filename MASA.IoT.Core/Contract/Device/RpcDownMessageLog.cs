using MASA.IoT.Common;

namespace MASA.IoT.Core.Contract.Device
{

    public class RpcDownMessageLog
    {
        /// <summary>
        /// 请求ID
        /// </summary>
        public Guid RequestId { get; set; }

        /// <summary>
        /// 消息体(业务接口最终返回的内容)
        /// </summary>
        [IsValue]

        public string? MessageData { get; set; }

    }

}
