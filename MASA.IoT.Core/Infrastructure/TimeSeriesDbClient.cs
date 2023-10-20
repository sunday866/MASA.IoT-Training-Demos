using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Core.Flux.Domain;
using InfluxDB.Client.Writes;
using MASA.IoT.Core.Contract.Device;
using MASA.IoT.Core.Contract.Enum;
using MASA.IoT.Core.Contract.Measurement;
using MASA.IoT.WebApi;
using Microsoft.Extensions.Options;
using System.Reflection;
using System.Text.RegularExpressions;
using MASA.IoT.Common;

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

        /// <summary>
        /// |> range(start: ""{option.StartDateTime}"",stop:""{option.StopDateTime}"")  
        /// </summary>             //and r._field == ""{option.FieldName}"" 
        /// <param name="option"></param>
        /// <returns></returns>
        public async Task<EChartsData> GetDeviceDataPointListAsync(GetDeviceDataPointListOption option)
        {
            var query =
                $@"from(bucket: ""{_bucket}"")
                    |> range(start: {option.UTCStartDateTimeStr},stop:{option.UTCStopDateTimeStr})                                                                                          
                    |> filter(fn: (r) => r._measurement == ""AirPurifierDataPoint"" 
                    and r.ProductId == ""{option.ProductId}"" 
                    and r.DeviceName == ""{option.DeviceName}"")
                    |> aggregateWindow(every: 2h, fn: mean)
                    |> fill(value: 0.0)";
            var tables = await _client.GetQueryApi().QueryAsync(query, _org);
            var fieldList = tables.SelectMany(table => table.Records).Select(o => o.GetField()).Distinct();
            var eChartsData = new EChartsData
            {
                DeviceName = option.DeviceName,
                FieldDataList = new List<FieldData>()
            };
            var fluxRecords = tables.SelectMany(table => table.Records);

            foreach (var field in fieldList)
            {
                eChartsData.FieldDataList.Add(new FieldData
                {
                    FieldName = field,
                    DateTimes = fluxRecords.Where(o => o.GetField() == field).Select(o => o.GetTime().Value.ToDateTimeUtc())
                        .ToList(),
                    Values = fluxRecords.Where(o => o.GetField() == field).Select(o => (double)o.GetValue()).ToList(),
                });
            }
            return eChartsData;
        }

        public async Task<List<T>> GetRecordListAsync<T>(string query) where T : new()
        {
            var tables = await _client.GetQueryApi().QueryAsync(query, _org);
            var fluxRecords = tables.SelectMany(table => table.Records).ToList();

            //获取所有属性。
            var properties = typeof(T).GetProperties();

            var result = new List<T>();
            foreach (var fluxRecord in fluxRecords)
            {
                var t = new T();
                foreach (var property in properties)
                {
                    var s = property;
                    var s1 = property.PropertyType;
                    var s2 = Type.GetTypeCode(s1);

                    if (property.PropertyType.Name == "Guid")
                    {
                        var v = fluxRecord.GetValueByKey(property.Name).ToString();

                        property.SetValue(t, Guid.Parse(v), null);

                    }
                    else if (property.PropertyType.IsEnum)
                    {
                        property.SetValue(t, int.Parse(fluxRecord.GetValueByKey(property.Name).ToString()), null);
                    }
                    else
                    {
                        if (property.CustomAttributes.Any(o => Attribute.IsDefined(property, typeof(IsValueAttribute))))
                        {
                            property.SetValue(t, fluxRecord.GetValue(), null);
                        }
                        else if (property.CustomAttributes.Any(o => Attribute.IsDefined(property, typeof(IsTsAttribute))))
                        {
                            property.SetValue(t, fluxRecord.GetTimeInDateTime(), null);
                        }
                        else
                        {
                            property.SetValue(t, fluxRecord.GetValueByKey(property.Name), null);
                        }
                    }

                    //switch (Type.GetTypeCode(property.GetType()))
                    //{
                    //    case TypeCode.Int32:

                    //        break;
                    //    case TypeCode.String:
                    //        property.SetValue(t, fluxRecord.GetValueByKey(property.Name).ToString(), null);
                    //        break;
                    //    case TypeCode.Object:
                    //        if (property is Guid)
                    //        {
                    //            property.SetValue(t, fluxRecord.GetValueByKey(property.Name).ToString(), null);
                    //        }
                    //     break;
                    //}

                }
                result.Add(t);
            }
            return result;

        }

        /// <summary>
        /// 从influxDb获取设备回复的消息
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        public async Task<(string messageId, string deviceResonse)> GetRpcMessageResultAsync(GetRpcMessageOption option)
        {
            var query =
                $@"from(bucket: ""{_bucket}"")
                    |> range(start: {option.UTCStartDateTimeStr},stop:{option.UTCStopDateTimeStr})                                                           
                    |> filter(fn: (r) => r._measurement == ""RpcMessage"" 
                    and r.MessageType == ""{(int)MessageType.Up}""
                    and r.RequestId == ""{option.RequestId}"")
                    |>last()";

            var tables = await _client.GetQueryApi().QueryAsync(query, _org);

            var fluxRecords = tables.SelectMany(table => table.Records);

            if (fluxRecords.Any())
            {
                return new ValueTuple<string, string>(fluxRecords.First().GetValueByKey("MessageId").ToString(),
                          fluxRecords.First().GetValue().ToString());
            }

            return new ValueTuple<string, string>(string.Empty, string.Empty);
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
                writeApi.WritePoint(point, _bucket, _org);
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

        static bool IsGuidByReg(string strSrc)
        {
            Regex reg = new Regex("^[A-F0-9]{8}(-[A-F0-9]{4}){3}-[A-F0-9]{12}$", RegexOptions.Compiled);
            return reg.IsMatch(strSrc);
        }


    }
}
