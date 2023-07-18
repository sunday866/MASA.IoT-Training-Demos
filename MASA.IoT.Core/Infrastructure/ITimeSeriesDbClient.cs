using MASA.IoT.Core.Contract;
using MASA.IoT.Core.Contract.Device;

namespace MASA.IoT.Core.Infrastructure
{
    public interface ITimeSeriesDbClient
    {
        bool WriteMeasurement<T>(T measurement);
        bool WriteMeasurements<T>(List<T> measurementList);
        bool WritePoint(AirPurifierDataPoint airPurifierDataPoint);
        Task<EChartsData> GetDeviceDataPointListAsync(GetDeviceDataPointListOption option);
    }
}
