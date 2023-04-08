using MASA.IoT.WebApi.Contract;
using MASA.IoT.WebApi.IHandler;
using MASA.IoT.WebApi.Models;
using Masa.BuildingBlocks.Data;
using Masa.BuildingBlocks.Data.Mapping;
using MASA.IoT.WebApi.Models.Models;

namespace MASA.IoT.WebApi.Handler
{
    public class DeviceHandler : IDeviceHandler
    {
        private readonly MASAIoTContext _ioTDbContext;
        private readonly IMapper _mapper;

        public DeviceHandler(MASAIoTContext ioTDbContext, IMapper mapper)
        {
            _ioTDbContext = ioTDbContext;
            _mapper = mapper;
        }

        public async Task<bool> DeviceRegAsync(DeviceRegRequest request)
        {
            var ioTDeviceInfo = _mapper.Map<IoTDeviceInfo>(request);
            ioTDeviceInfo.Id = new Guid();
            await _ioTDbContext.IoTDeviceInfo.AddAsync(ioTDeviceInfo);
            await _ioTDbContext.SaveChangesAsync();
            return true;
        }

        private async Task<string> GenerateDeviceNameAsync(string productCode, string uuid)
        {
            var lastDeviceware = _ioTDbContext.IoTDevicewares.LastOrDefault(o => o.ProductCode == productCode);

            var newDeviceware = new IoTDevicewares
            {
                Id = Guid.NewGuid(),
                UUID = uuid,
                ProductCode = productCode
            };

            if (lastDeviceware != null)
            {
                newDeviceware.DeviceName = (int.Parse(lastDeviceware.DeviceName) + 1).ToString(),
            }
            else
            {
                newDeviceware.DeviceName = productCode + "00000001";
            }
            await _ioTDbContext.IoTDevicewares.AddAsync(newDeviceware);
            await _ioTDbContext.SaveChangesAsync();

            return newDeviceware.DeviceName;
        }
    }
}
