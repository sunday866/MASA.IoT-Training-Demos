using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Formatter;

namespace MASA.IoT.Common.Helper
{
    public class MqttHelper
    {
        private MqttFactory _mqttFactory;
        private IMqttClient _mqttClient;
        private MqttClientOptions _mqttClientOptions;
        private MqttClientSubscribeOptions _mqttClientSubscribeOptions;

        public MqttHelper(string mqttUrl, string clientID, string userName, string passWord)
        {
            _mqttFactory = new MqttFactory();
            _mqttClient = _mqttFactory.CreateMqttClient();
            _mqttClientOptions = new MqttClientOptionsBuilder()
                                  .WithTcpServer(mqttUrl)
              .WithCredentials(userName, passWord).WithProtocolVersion(MqttProtocolVersion.V500).Build();

            _mqttClientOptions.ClientId = clientID;
        }


        //public async Task SendCmdAsync(string productKey,string deviceName, string requestId,string stringData)  
        //{
        //    if (!_mqttClient.IsConnected)
        //    {
        //        await _mqttClient.ConnectAsync(_mqttClientOptions, CancellationToken.None);
        //    }
        //    await _mqttClient.PublishStringAsync($"rpc/{productKey}/{deviceName}/{requestId}", stringData);
        //}

        public async Task PublishStringAsync(string topic, string stringData)
        {
            if (!_mqttClient.IsConnected)
            {
                await _mqttClient.ConnectAsync(_mqttClientOptions, CancellationToken.None);
            }
            await _mqttClient.PublishStringAsync($"{topic}", stringData);
        }

        /// <summary>
        /// 连接并订阅Topic
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="topic"></param>
        /// <returns></returns>
        public async Task ConnectClient(Func<MqttApplicationMessageReceivedEventArgs, Task> callback, string topic)
        {
            _mqttClientSubscribeOptions = _mqttFactory.CreateSubscribeOptionsBuilder()
                .WithTopicFilter(f => { f.WithTopic(topic); })
                .Build();

            var response = await _mqttClient.ConnectAsync(_mqttClientOptions, CancellationToken.None);
            if (response.ResultCode == MqttClientConnectResultCode.Success)
            {
                Console.WriteLine($"The MQTT client with topic:{topic} is connected.");
                await Task.Delay(500);
                _mqttClient.ApplicationMessageReceivedAsync += callback;
                await _mqttClient.SubscribeAsync(_mqttClientSubscribeOptions, CancellationToken.None);
            }
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        /// <returns></returns>
        public async Task Disconnect_Client()
        {
            if (_mqttClient.IsConnected)
            {
                await _mqttClient.DisconnectAsync();

                Console.WriteLine("The MQTT client is Disconnected.");
            }
        }
    }
}
