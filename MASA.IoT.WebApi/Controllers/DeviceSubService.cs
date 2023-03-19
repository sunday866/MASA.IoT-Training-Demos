using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Mvc;
using Dapr;
using Dapr.Client;
using MASA.IoT.Common;

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

