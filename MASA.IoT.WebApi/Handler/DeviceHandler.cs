using MASA.IoT.WebApi.Contract;
using MASA.IoT.WebApi.IHandler;
using MASA.IoT.WebApi.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace MASA.IoT.WebApi.Handler
{
    public class DeviceHandler : IDeviceHandler
    {
        private readonly MASAIoTContext _ioTDbContext;
        private readonly IMqttHandler _mqttHandler;

        public DeviceHandler(MASAIoTContext ioTDbContext, IMqttHandler mqttHandler)
        {
            _ioTDbContext = ioTDbContext;
            _mqttHandler = mqttHandler;
        }

        /// <summary>
        /// 注册设备
        /// </summary>
        /// <param name="request"></param>
        /// <returns>
        /// 设备注册信息
        /// </returns>
        public async Task<DeviceRegResponse> DeviceRegAsync(DeviceRegRequest request)
        {

            var productInfo =
                await _ioTDbContext.IoTProductInfo.FirstOrDefaultAsync(o => o.ProductCode == request.ProductCode);
            if (productInfo == null)
            {
                return new DeviceRegResponse
                {

                    Succeed = false,
                    ErrMsg = "ProductCode not found"
                };
            }
            var deviceRegInfo = await GetDeviceRegInfoAsync(request);
            if (deviceRegInfo != null) //已经注册过
            {
                return deviceRegInfo;
            }
            else //没有注册过
            {
                var deviceName = await GenerateDeviceNameAsync(productInfo.SupplyNo, request.ProductCode, request.UUID);
                var password = Guid.NewGuid().ToString("N");
                var addDeviceResponse = await _mqttHandler.DeviceRegAsync(deviceName, password);
                if (addDeviceResponse.User_id == deviceName) //注册成功
                {
                    deviceRegInfo = new DeviceRegResponse
                    {
                        DeviceName = deviceName,
                        Password = password,
                        Succeed = false,
                        ErrMsg = null
                    };
                    await _ioTDbContext.IoTDeviceInfo.AddAsync(new IoTDeviceInfo
                    {
                        Id = Guid.NewGuid(),
                        DeviceName = deviceName,
                        Password = password,
                        ProductInfoId = productInfo.Id,
                    });
                    await _ioTDbContext.SaveChangesAsync();
                    return deviceRegInfo;
                }

                return new DeviceRegResponse
                {
                    Succeed = false,
                    ErrMsg = addDeviceResponse.Message
                };
            }

        }

        /// <summary>
        /// 获取设备注册信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns>
        /// 设备已经注册返回设备注册信息，没有注册过返回null
        /// </returns>
        private async Task<DeviceRegResponse?> GetDeviceRegInfoAsync(DeviceRegRequest request)
        {
            var deviceware = await _ioTDbContext.IoTDevicewares.FirstOrDefaultAsync(o => o.ProductCode == request.ProductCode && o.UUID == request.UUID);

            if (deviceware == null)
            {
                return null;
            }
            else
            {
                var deviceInfo = await _ioTDbContext.IoTDeviceInfo.FirstAsync(o => o.DeviceName == deviceware.DeviceName);

                return new DeviceRegResponse
                {
                    DeviceName = deviceInfo.DeviceName,
                    Password = deviceInfo.Password,
                    Succeed = true,
                    ErrMsg = string.Empty
                };
            }
        }

        /// <summary>
        /// 生成设备名称
        /// </summary>
        /// <param name="productCode"></param>
        /// <param name="uuid"></param>
        /// <returns>
        /// 设备Mqtt名称
        /// </returns>
        private async Task<string> GenerateDeviceNameAsync(string supplyNo,string productCode, string uuid)
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
                newDeviceware.DeviceName = (int.Parse(lastDeviceware.DeviceName) + 1).ToString();
            }
            else
            {
                newDeviceware.DeviceName = supplyNo + "00000001";
            }
            await _ioTDbContext.IoTDevicewares.AddAsync(newDeviceware);
            await _ioTDbContext.SaveChangesAsync();

            return newDeviceware.DeviceName;
        }
    }
}
