using Dapr.Client;
using MASA.IoT.Common;
using MASA.IoT.Common.Helper;
using Microsoft.Extensions.Options;
using MQTTnet.Client;
using Newtonsoft.Json.Linq;

namespace MASA.IoT.Hub;

public class MQHostedService : IHostedService
{

    private readonly HubAppSettings _appSettings;
    private readonly Dapr.Client.DaprClient daprClient;
    public MQHostedService(
        IOptions<HubAppSettings> appSettings)
    {
        daprClient = new DaprClientBuilder().Build();
        _appSettings = appSettings.Value;
    }

    /// <summary>
    /// ¿ªÊ¼                
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var mqttHelper = new MqttHelper(_appSettings.MqttSetting.MqttUrl, "IoTHub", _appSettings.MqttSetting.UserName, _appSettings.MqttSetting.Password);
        var daprClient = new DaprClientBuilder().Build();
        await mqttHelper.Connect_Client_Using_WebSockets(CallbackAsync, _appSettings.MqttSetting.Topic);
        Console.ReadKey();
    }
    private async Task CallbackAsync(MqttApplicationMessageReceivedEventArgs e)
    {
        var deviceDataPointStr = System.Text.Encoding.Default.GetString(e.ApplicationMessage.Payload);
        Console.WriteLine(deviceDataPointStr);
        var pubSubOptions = new PubSubOptions
        {
            DeviceOneNetId = "123",
            Msg = deviceDataPointStr,
            Timestamp = 123,
            PubTime = new DateTimeOffset(DateTime.Now).ToUnixTimeMilliseconds(),
            TrackId = Guid.NewGuid()
        };
        try
        {
            await daprClient.PublishEventAsync("pubsub", "newOrder", pubSubOptions);
        }
        catch (Exception ex)
        {

            Console.WriteLine(ex.Message);
        }

    }
    /// <summary>
    /// ½áÊø
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
