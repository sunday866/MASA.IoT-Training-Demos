using InfluxDB.Client.Core;

namespace MASA.IoT.Core.Contract
{
    [Measurement("AirPurifierDataPoint")]
    public class AirPurifierDataPoint
    {
        /// <summary>
        /// 设备名称
        /// </summary>
        [Column("DeviceName", IsTag = true)] public string DeviceName { get; set; }

        /// <summary>
        /// 产品ID
        /// </summary>
        [Column("ProductId", IsTag = true)] public Guid ProductId { get; set; }

        /// <summary>
        /// Pm2.5
        /// </summary>
        [Column("PM_25")] public double? Pm_25 { get; set; }
        /// <summary>
        /// 温度
        /// </summary>
        [Column("Temperature")] public double? Temperature { get; set; }
        /// <summary>
        /// 湿度
        /// </summary>
        [Column("Humidity")] public double? Humidity { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        [Column(IsTimestamp = true)] public DateTime Time { get; set; }
    }
}
