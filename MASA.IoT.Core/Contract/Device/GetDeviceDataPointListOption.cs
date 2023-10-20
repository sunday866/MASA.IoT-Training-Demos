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
                var newt = DateTime.SpecifyKind(StartDateTime.Value, DateTimeKind.Local);
                var utcStartTime = TimeZoneInfo.ConvertTimeToUtc(newt, TimeZoneInfo.Local);
                return utcStartTime.ToString("yyyy-MM-ddTHH:mm:ssZ");
            }
        }

        public DateTime? StopDateTime { get; set; }

        public string UTCStopDateTimeStr
        {
            get
            {
                var newt2 = DateTime.SpecifyKind(StopDateTime.Value, DateTimeKind.Local);
                var utcStartTime = TimeZoneInfo.ConvertTimeToUtc(newt2, TimeZoneInfo.Local);
                return utcStartTime.ToString("yyyy-MM-ddTHH:mm:ssZ");
            }
        }
    }
}
