using System.Net;
using Flurl.Http;
using MASA.IoT.Core.Contract.Mqtt;
using MASA.IoT.Core.IHandler;
using MASA.IoT.WebApi;
using MASA.IoT.WebApi.Contract.Mqtt;
using Microsoft.Extensions.Options;

namespace MASA.IoT.Core.Handler
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

        /// <summary>
        /// 向EMQX发布消息
        /// </summary>
        /// <returns></returns>
        public async Task<EmqxBaseResponse> PublishToMqttAsync(PublishMessageRequest request)
        {
            var url = $"{_appSettings.MqttSetting.Url}/api/v5/publish";
            var response = await url.WithBasicAuth(_appSettings.MqttSetting.ApiKey, _appSettings.MqttSetting.SecretKey).AllowAnyHttpStatus().PostJsonAsync(request);
            if (response.StatusCode is (int)HttpStatusCode.OK //200
                or (int)HttpStatusCode.BadRequest //400
                or (int)HttpStatusCode.ServiceUnavailable //503
                or (int)HttpStatusCode.Accepted) //202
            {
                return await response.GetJsonAsync<EmqxBaseResponse>();
            }
            else
            {
                throw new UserFriendlyException(await response.GetStringAsync());
            }
        }
    }
}
