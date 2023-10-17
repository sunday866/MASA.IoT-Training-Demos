
namespace MASA.IoT.Core.Contract.Device
{
    public class GetRpcMessageOption
    {
        public Guid RequestId { get; set; }

        public DateTime? StartDateTime { get; set; }

        public int Timeout { get; set; } = 5;

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
