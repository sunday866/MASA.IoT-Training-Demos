using Flurl.Http;
using MASA.IoT.Common;
using MASA.IoT.Core.GateWay;
using Microsoft.Extensions.Options;
using System.Net;

namespace MASA.IoT.WebApi.Handler
{
    public class RulesEngineGateWay : IRulesEngineGateWay
    {
        private readonly AppSettings _appSettings;
        public RulesEngineGateWay(IOptions<AppSettings> settings)
        {
            _appSettings = settings.Value;
        }

        public async Task<bool> SendDataAsync(PubSubOptions pubSubOptions)
        {
            var url = $"{_appSettings.nodeREDSetting.Url}/api/msg-data";
            var response = await url.WithHeader("Content-Type", "application/json; charset=utf-8").PostStringAsync(pubSubOptions.Msg);
            if (response.StatusCode is (int)HttpStatusCode.OK)
            {
                return true;
            }
            else
            {
                throw new UserFriendlyException(await response.GetStringAsync());
            }
        }
    }
}
