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
    [ApiController]
    public class CheckoutServiceController : Controller
    {
        //Subscribe to a topic 
        [Topic("pubsub", "datapoint")]
        [HttpPost("datapoint")]
        public void getCheckout([FromBody] PubSubOptions pubSubOptions)
        {
            Console.WriteLine("Subscriber received : " + pubSubOptions.Msg);
        }

    }
}

