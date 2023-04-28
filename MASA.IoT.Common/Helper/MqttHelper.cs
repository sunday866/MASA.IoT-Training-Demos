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
        public MqttHelper(string mqttUrl, string clientID,string userName,string passWord)
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
            await mqttClient.PublishStringAsync($"samples/topic/cmd{topicIndex}", stringdata);
        }
        public async Task ConnectClient(Func<MqttApplicationMessageReceivedEventArgs, Task> callback, string topic) //订阅客户端
        {
            /*
             * This sample creates a simple MQTT client and connects to a public broker using a WebSocket connection.
             * 
             * This is a modified version of the sample _Connect_Client_! See other sample for more details.
             */
            mqttClientSubscribeOptions = mqttFactory.CreateSubscribeOptionsBuilder()
                //.WithTopicFilter(f => { f.WithTopic($"v1/devices/{userName}/telemetry"); })
                .WithTopicFilter(f => { f.WithTopic(topic); })
                .Build();

            var response = await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

            Console.WriteLine($"The MQTT client with topic:{topic} is connected.");

            await Task.Delay(500);
            //mqttClient.ApplicationMessageReceivedAsync -= callback;
            mqttClient.ApplicationMessageReceivedAsync += callback;
            var response2 = await mqttClient.SubscribeAsync(mqttClientSubscribeOptions, CancellationToken.None);
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
