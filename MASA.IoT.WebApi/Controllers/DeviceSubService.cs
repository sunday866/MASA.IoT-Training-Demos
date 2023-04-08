using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Mvc;
using Dapr;
using Dapr.Client;
using Masa.BuildingBlocks.Ddd.Domain.Repositories;
using MASA.IoT.Common;
using MASA.IoT.WebApi.Models;
using MASA.IoT.WebApi.Models.Models;

namespace MASA.IoT.WebApi.Controllers
{
    public class DeviceSubService : ServiceBase
    {
        public DeviceSubService(IServiceCollection services):base(services, "/api/DeviceSub")
        {
            var apiPath = "/api/BusinessDeviceSub";
            App.MapPost($"{apiPath}/{nameof(BusinessMQOperation)}", BusinessMQOperation);
        }

        [Topic("pubsub", "BusinessMQOperation")]
        public async Task BusinessMQOperation(PubSubOptions options)
        {
            Console.WriteLine(options.Msg);
        }

    }
}

