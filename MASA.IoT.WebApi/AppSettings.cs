namespace MASA.IoT.WebApi
{
    public class AppSettings
    {

        public MqttSetting MqttSetting { get; set; }
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
    }
}
