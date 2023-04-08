using System.Runtime.Serialization;

namespace MASA.IoT.WebApi.Contract.Mqtt
{
    public class AddDeviceRequest
    {

        public string User_id { get; set; }
        public string Password { get; set; }
    }
}
