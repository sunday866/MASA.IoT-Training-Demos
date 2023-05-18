using Dapr.Client;
using MASA.IoT.Common;
using MASA.IoT.Common.Helper;
using Microsoft.Extensions.Options;
using MQTTnet.Client;

namespace MASA.IoT.Hub;

public class MQHostedService : IHostedService
{
    private readonly HubAppSettings _appSettings;
    private readonly DaprClient _daprClient;
    public MQHostedService(IOptions<HubAppSettings> appSettings)
    {
        _daprClient = new DaprClientBuilder().Build();
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
 
        await mqttHelper.ConnectClient(CallbackAsync, _appSettings.MqttSetting.Topic);

    }
    private async Task CallbackAsync(MqttApplicationMessageReceivedEventArgs e)
    {
        var deviceDataPointStr = System.Text.Encoding.Default.GetString(e.ApplicationMessage.PayloadSegment);

        Console.WriteLine(deviceDataPointStr);
        var pubSubOptions = new PubSubOptions
        {
            DeviceName = e.ApplicationMessage.Topic[6..^3],
            Msg = deviceDataPointStr,
            PubTime = new DateTimeOffset(DateTime.Now).ToUnixTimeMilliseconds(),
            TrackId = Guid.NewGuid()
        };                            
        try
        {
            await _daprClient.PublishEventAsync("pubsub", "DeviceMessage", pubSubOptions);
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
