namespace MASA.IoT.Hub
{
    public class HubAppSettings
    {
        public MqttSetting MqttSetting { get; set; }

        /// <summary>
        /// 环境
        /// </summary>
        public string EnvironmentName { get; set; }

        /// <summary>
        /// 是否生产环境
        /// </summary>
        public bool IsProduction => "Production".Equals(EnvironmentName);
    }


    public class MqttSetting
    {
        public string MqttUrl { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Topic { get; set; }
    }

}
