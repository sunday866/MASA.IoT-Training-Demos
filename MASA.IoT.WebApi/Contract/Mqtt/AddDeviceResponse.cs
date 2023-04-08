using System.Runtime.Serialization;

namespace MASA.IoT.WebApi.Contract.Mqtt
{
    public class AddDeviceResponse
    {
        //[DataMember(Name="code")]
        public string code { get; set; }
        //[DataMember(Name = "message")] 
        public string message { get; set; }
        //[DataMember(Name = "user_id")] 
        public string user_id { get; set; }

    }

}
