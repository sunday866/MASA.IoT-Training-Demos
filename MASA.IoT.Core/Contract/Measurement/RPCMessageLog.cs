using InfluxDB.Client.Core;

namespace MASA.IoT.Core.Contract.Measurement
{
    [InfluxDB.Client.Core.Measurement("RPCMessageLog")]
    public class RPCMessageLog
    {
        /// <summary>
        /// 请求ID
        /// </summary>
        [Column("RequestId", IsTag = true)] public Guid RequestId { get; set; }

        /// <summary>
        /// 业务接口最终返回的内容
        /// </summary>
        [Column("RpcMessageResponseJson")]
        public string? RpcMessageResponseJson { get; set; }
    }

}
