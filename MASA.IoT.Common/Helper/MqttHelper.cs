using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Formatter;

namespace MASA.IoT.Common.Helper
{
    public class MqttHelper
    {
        private MqttFactory mqttFactory;
        private IMqttClient mqttClient;
        private MqttClientOptions mqttClientOptions;
        private MqttClientSubscribeOptions mqttClientSubscribeOptions;
        public MqttHelper(string mqttUrl, string clientID, string userName, string passWord)
        {
            mqttFactory = new MqttFactory();
            mqttClient = mqttFactory.CreateMqttClient();
            mqttClientOptions = new MqttClientOptionsBuilder()
                                  .WithTcpServer(mqttUrl)
              .WithCredentials(userName, passWord).WithProtocolVersion(MqttProtocolVersion.V500).Build();

            mqttClientOptions.ClientId = clientID;
        }


        public async Task SendCmdAsync(string stringdata, string topicIndex)  //发布客户端的消息
        {
            if (!mqttClient.IsConnected)
            {
                await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);
            }
            await mqttClient.PublishStringAsync($"topic/cmd{topicIndex}", stringdata);
        }
        public async Task ConnectClient(Func<MqttApplicationMessageReceivedEventArgs, Task> callback, string topic) //连接并订阅客户端
        {
            mqttClientSubscribeOptions = mqttFactory.CreateSubscribeOptionsBuilder()
                .WithTopicFilter(f => { f.WithTopic(topic); })
                .Build();

            var response = await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);
            if (response.ResultCode == MqttClientConnectResultCode.Success)
            {
                Console.WriteLine($"The MQTT client with topic:{topic} is connected.");
                await Task.Delay(500);
                mqttClient.ApplicationMessageReceivedAsync += callback;
                await mqttClient.SubscribeAsync(mqttClientSubscribeOptions, CancellationToken.None);
            }
        }

        public async Task Disconnect_Client() //订阅客户端
        {
            if (mqttClient.IsConnected)
            {
                await mqttClient.DisconnectAsync();

                Console.WriteLine("The MQTT client is Disconnected.");
            }
        }
    }
}
