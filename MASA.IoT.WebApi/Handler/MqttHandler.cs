using Flurl.Http;
using MASA.IoT.WebApi.Contract.Mqtt;
using MASA.IoT.WebApi.IHandler;
using Microsoft.AspNetCore.Components;
using System.Net;

namespace MASA.IoT.WebApi.Handler
{
    public class MqttHandler : IMqttHandler
    {
        [Inject]
        public AppSettings AppSettings { get; set; }
        public async Task<AddDeviceResponse> DeviceRegAsync(string DeviceName)
        {
            var url = $"{AppSettings.MqttSetting.Url}/api/v5/authentication/password_based%3Abuilt_in_database/users";
            var response = await url.WithBasicAuth(AppSettings.MqttSetting.ApiKey, AppSettings.MqttSetting.SecretKey).AllowAnyHttpStatus().PostJsonAsync(new AddDeviceRequest
            {
                User_id = DeviceName,
                Password = Guid.NewGuid().ToString("N"),
            }
            );
            if (response.StatusCode is (int)HttpStatusCode.Created or (int)HttpStatusCode.BadRequest or (int)HttpStatusCode.NotFound)
            {
                return await response.GetJsonAsync<AddDeviceResponse>();
            }
            else
            {
                throw new UserFriendlyException(await response.GetStringAsync());
            }
        }
    }
}
