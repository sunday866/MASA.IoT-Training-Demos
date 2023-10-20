using Microsoft.AspNetCore.Mvc;
using Dapr;
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

