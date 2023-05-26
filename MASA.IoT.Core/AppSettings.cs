namespace MASA.IoT.WebApi
{
    public class AppSettings
    {

        public MqttSetting MqttSetting { get; set; }
        public InfluxDBSetting InfluxDBSetting { get; set; }
        public NodeREDSetting nodeREDSetting { get; set; }
        /// <summary>
        /// 环境
        /// </summary>
        public string EnvironmentName { get; set; }
    }

    public class MqttSetting
    {
        public string Url { get; set; }
        public string ApiKey { get; set; }
        public string SecretKey { get; set; }
        public string JwtSecret { get; set; }
    }
    public class InfluxDBSetting
    {
        public string Url { get; set; }
        public string Token { get; set; }
        public string Bucket { get; set; }
        public string Org { get; set; }
    }
    public class NodeREDSetting
    {
        public string Url { get; set; }
    }

}
