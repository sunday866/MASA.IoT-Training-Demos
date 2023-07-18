namespace MASA.IoT.Core.Contract.Device
{
    public class EChartsData
    {
        /// <summary>
        /// 设备名称
        /// </summary>
        public string DeviceName { get; set; }

        public List<FieldData> FieldDataList { get; set; }
    }

    public class FieldData
    {
        /// <summary>
        /// 字段名称
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// 时间点列表（X轴）
        /// </summary>
        public List<DateTime> DateTimes { get; set; }

        /// <summary>
        /// 数据点列表（Y轴）
        /// </summary>
        public List<double> Values { get; set; }
    }
}
