namespace MASA.IoT.Core.Contract.Device
{
    public class GetDeviceDataPointListOption
    {
        public Guid ProductId { get; set; }

        public string DeviceName { get; set; }

        //public string FieldName { get; set; }
        public DateTime? StartDateTime { get; set; }

        public string UTCStartDateTimeStr
        {
            get
            {
                var utcStartTime = TimeZoneInfo.ConvertTimeToUtc(StartDateTime.Value, TimeZoneInfo.Local);
                return utcStartTime.ToString("yyyy-MM-ddTHH:mm:ssZ");
            }
        }

        public DateTime? StopDateTime { get; set; }

        public string UTCStopDateTimeStr
        {
            get
            {
                var utcStartTime = TimeZoneInfo.ConvertTimeToUtc(StopDateTime.Value, TimeZoneInfo.Local);
                return utcStartTime.ToString("yyyy-MM-ddTHH:mm:ssZ");
            }
        }
    }
}
