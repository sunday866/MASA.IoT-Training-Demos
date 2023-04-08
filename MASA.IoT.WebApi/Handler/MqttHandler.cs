using Flurl.Http;
using MASA.IoT.WebApi.Contract.Mqtt;
using MASA.IoT.WebApi.IHandler;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using System.Net;

namespace MASA.IoT.WebApi.Handler
{
    public class MqttHandler : IMqttHandler
    {
        private readonly AppSettings _appSettings;
        public MqttHandler(IOptions<AppSettings> settings)
        {
            _appSettings = settings.Value;
        }

        public async Task<AddDeviceResponse> DeviceRegAsync(string deviceName,string password)
        {
            var url = $"{_appSettings.MqttSetting.Url}/api/v5/authentication/password_based:built_in_database/users";
            var response = await url.WithBasicAuth(_appSettings.MqttSetting.ApiKey, _appSettings.MqttSetting.SecretKey).AllowAnyHttpStatus().PostJsonAsync(new AddDeviceRequest
            {
                user_id = deviceName,
                password = password,
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
