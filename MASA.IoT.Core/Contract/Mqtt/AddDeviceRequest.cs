namespace MASA.IoT.WebApi.Contract.Mqtt
{
    //[DataContract]
    public class AddDeviceRequest
    {
        //[DataMember(Name= "user_Id")]
        public string user_id { get; set; }
        //[DataMember(Name = "password")] 
        public string password { get; set; }
    }
}
