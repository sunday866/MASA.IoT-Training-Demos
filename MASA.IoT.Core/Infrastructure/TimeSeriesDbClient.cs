using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;
using MASA.IoT.Core.Contract;
using MASA.IoT.WebApi;
using Microsoft.Extensions.Options;

namespace MASA.IoT.Core.Infrastructure
{
    public class TimeSeriesDbClient : ITimeSeriesDbClient
    {
        private readonly InfluxDBClient _client;
        private readonly string _bucket;
        private readonly string _org;
        private readonly AppSettings _appSettings;
        
        public TimeSeriesDbClient(IOptions<AppSettings> settings)
        {
            _appSettings = settings.Value;
            _org = _appSettings.InfluxDBSetting.Org;
            _bucket = _appSettings.InfluxDBSetting.Bucket;
            _client = new InfluxDBClient(_appSettings.InfluxDBSetting.Url, _appSettings.InfluxDBSetting.Token);
        }

        public bool WriteMeasurement<T>(T measurement)
        {
            try
            {
                using var writeApi = _client.GetWriteApi();
                writeApi.WriteMeasurement<T>(measurement, WritePrecision.Ms, _bucket, _org);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public bool WritePoint(AirPurifierDataPoint airPurifierDataPoint)
        {
            try
            {
                var point = PointData
                    .Measurement("AirPurifierDataPoint")
                    .Tag("DeviceName", airPurifierDataPoint.DeviceName)
                    .Tag("ProductId", airPurifierDataPoint.ProductId.ToString())
                    .Field("PM_25", airPurifierDataPoint.Pm_25)
                    .Field("Temperature", airPurifierDataPoint.Temperature)
                    .Field("Humidity", airPurifierDataPoint.Humidity)
                    .Timestamp(airPurifierDataPoint.Timestamp, WritePrecision.Ms);
                using var writeApi = _client.GetWriteApi();
                writeApi.WritePoint(point,_bucket, _org);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public bool WriteMeasurements<T>(List<T> measurementList)
        {
            try
            {
                using var writeApi = _client.GetWriteApi();
                writeApi.WriteMeasurements<T>(measurementList, WritePrecision.Ms, _bucket, _org);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
