using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Formatter;

namespace MASA.IoT.AdminPortal.Helper {
    public class MqttHelper {
        private MqttFactory mqttFactory;
        private IMqttClient mqttClient;
        private MqttClientOptions mqttClientOptions;
        private MqttClientSubscribeOptions mqttClientSubscribeOptions;
        public MqttHelper(string mqttUrl, string clientID) {
            mqttFactory = new MqttFactory();
            mqttClient = mqttFactory.CreateMqttClient();
            mqttClientOptions = new MqttClientOptionsBuilder()
                                  .WithTcpServer(mqttUrl, 2883)
              .WithCredentials("emqx", "123456").WithProtocolVersion(MqttProtocolVersion.V500).Build();

            mqttClientOptions.ClientId = clientID;
        }


        public async Task SendCmdAsync(string stringdata, string topicIndex)  //发布客户端的消息
        {
            if (!mqttClient.IsConnected) {
                await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);
            }
            await mqttClient.PublishStringAsync($"samples/topic/cmd{topicIndex}", stringdata);
        }
        public async Task Connect_Client_Using_WebSockets(Func<MqttApplicationMessageReceivedEventArgs, Task> callback, string userName) //订阅客户端
        {
            /*
             * This sample creates a simple MQTT client and connects to a public broker using a WebSocket connection.
             * 
             * This is a modified version of the sample _Connect_Client_! See other sample for more details.
             */
            mqttClientSubscribeOptions = mqttFactory.CreateSubscribeOptionsBuilder()
                .WithTopicFilter(f => { f.WithTopic($"v1/devices/{userName}/telemetry"); })
                .Build();

            var response = await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

            Console.WriteLine("The MQTT client is connected.");

            await Task.Delay(500);
            mqttClient.ApplicationMessageReceivedAsync -= callback;
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
