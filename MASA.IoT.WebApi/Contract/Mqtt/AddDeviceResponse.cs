using System.Runtime.Serialization;

namespace MASA.IoT.WebApi.Contract.Mqtt
{
    public class AddDeviceResponse
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public string User_id { get; set; }

    }

}
