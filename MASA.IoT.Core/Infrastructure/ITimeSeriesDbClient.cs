using MASA.IoT.Core.Contract.Device;
using MASA.IoT.Core.Contract.Measurement;

namespace MASA.IoT.Core.Infrastructure
{
    public interface ITimeSeriesDbClient
    {
        bool WriteMeasurement<T>(T measurement);
        bool WriteMeasurements<T>(List<T> measurementList);
        bool WritePoint(AirPurifierDataPoint airPurifierDataPoint);
        Task<EChartsData> GetDeviceDataPointListAsync(GetDeviceDataPointListOption option);

        Task<(string messageId, string deviceResonse)> GetRpcMessageResultAsync(GetRpcMessageOption option);

        Task<List<T>> GetRecordListAsync<T>(string query) where T : new();
    }
}
