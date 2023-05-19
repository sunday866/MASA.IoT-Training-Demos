using MASA.IoT.Core.Contract;

namespace MASA.IoT.Core.Infrastructure
{
    public interface ITimeSeriesDbClient
    {
        bool WriteMeasurement<T>(T measurement);
        bool WriteMeasurements<T>(List<T> measurementList);
        bool WritePoint(AirPurifierDataPoint airPurifierDataPoint);
    }
}
