using Dapr.Client.Autogen.Grpc.v1;
using Masa.BuildingBlocks.Ddd.Domain.Repositories;
using MASA.IoT.Core.Contract.Device;
using MASA.IoT.Core.Contract.Enum;
using MASA.IoT.Core.IHandler;
using MASA.IoT.Core.Infrastructure;
using MASA.IoT.WebApi.Contract;
using MASA.IoT.WebApi.IHandler;
using MASA.IoT.WebApi.Models.Models;
using Masa.Utils.Models;
using Microsoft.EntityFrameworkCore;
using MASA.IoT.Common;
using MASA.IoT.Core.Contract.Measurement;
using Newtonsoft.Json;
using MASA.IoT.Core.Contract.Mqtt;

namespace MASA.IoT.Core.Handler
{
    public class DeviceHandler : IDeviceHandler
    {
        private readonly MASAIoTContext _ioTDbContext;
        private readonly IMqttHandler _mqttHandler;
        private readonly ITimeSeriesDbClient _timeSeriesDbClient;

        public DeviceHandler(MASAIoTContext ioTDbContext, IMqttHandler mqttHandler, ITimeSeriesDbClient timeSeriesDbClient)
        {
            _ioTDbContext = ioTDbContext;
            _mqttHandler = mqttHandler;
            _timeSeriesDbClient = timeSeriesDbClient;
        }


        /// <summary>
        /// 写入数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pubSubOptions"></param>
        /// <returns></returns>
        public async Task<bool> WriteMeasurementAsync<T>(PubSubOptions pubSubOptions)
        {
            var device = await _ioTDbContext.IoTDeviceInfo.Include(o => o.ProductInfo).AsNoTracking()
                .FirstOrDefaultAsync(o => o.DeviceName == pubSubOptions.DeviceName);

            if (device != null && device.ProductInfo.ProductCode == "10001")  //空气净化器产品
            {
                var airPurifierDataPoint = JsonConvert.DeserializeObject<AirPurifierDataPoint>(pubSubOptions.Msg);

                airPurifierDataPoint.ProductId = device.ProductInfoId;

                return _timeSeriesDbClient.WriteMeasurement<AirPurifierDataPoint>(airPurifierDataPoint);
                //return _timeSeriesDbClient.WritePoint(airPurifierDataPoint);

            }
            return false;
        }

        public async Task<EChartsData> GetDeviceDataPointListAsync(GetDeviceDataPointListOption option)
        {
            return await _timeSeriesDbClient.GetDeviceDataPointListAsync(option);
        }


        public async Task<RpcMessageResponse> PublishAndGetResponseAsync(RpcMessageRequest request)
        {

        }

        /// <summary>
        /// 写入RPC日志
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public bool WriteRpcMessage(RpcMessageRequest request)
        {
            var message = new RPCMessage
            {
                DeviceName = request.DeviceName,
                ProductId = request.ProductId,
                MessageType = MessageType.Down,
                RequestId = Guid.NewGuid(),
                MessageId = Guid.NewGuid(),
                MessageData = request.MessageData,
                Timestamp = Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalMilliseconds)
            };
            //记录下发指令
            return _timeSeriesDbClient.WriteMeasurement(message);
        }


        private async Task<RpcMessageResponse> GetRpcMessageResponseAsync(GetRpcMessageOption option)
        {
            var deviceResponse = string.Empty;
            for (int i = 0; i < option.Timeout * 10; i++) //100ms查询一次
            {
                deviceResponse = await _timeSeriesDbClient.GetRpcMessageResultAsync(option);
                if (!string.IsNullOrEmpty(deviceResponse))
                {
                    break; //查询到设备返回消息就停止
                }
                await Task.Delay(100);
            }

            var result = new RpcMessageResponse();
            if (!string.IsNullOrEmpty(deviceResponse))
            {
                var rpcMessageResponse = JsonConvert.DeserializeObject<RpcMessageResponse>(deviceResponse);
                result.Success = rpcMessageResponse.Success;
                result.ErrorMessage = rpcMessageResponse.ErrorMessage;
                return rpcMessageResponse;
            }
            else
            {
                return new RpcMessageResponse //查询不到返回超时
                {
                    Success = false,
                    ErrorMessage = "Cmd Timeout",
                    MessageId = option.MessageId,
                };
            }
        }

        public Task<bool> WriteTestDataAsync()
        {
            var productId = Guid.Parse("c85ef7e5-2e43-4bd2-a939-07fe5ea3f459");
            var second = 0;

            for (int i = 0; i < 86400; i++)
            {
                var timestamp =
                    Convert.ToInt64((DateTime.UtcNow.AddSeconds(i * 5) -
                                     new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalMilliseconds);
                _timeSeriesDbClient.WriteMeasurement<AirPurifierDataPoint>(new AirPurifierDataPoint
                {
                    DeviceName = "284202304230001",
                    ProductId = productId,
                    Pm_25 = new Random().Next(5, 45),
                    Temperature = new Random().Next(10, 40),
                    Humidity = new Random().Next(10, 99),
                    Timestamp = timestamp
                });
                second++;
            }
            return Task.FromResult(true);

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
                if (addDeviceResponse.user_id == deviceName) //注册成功
                {
                    deviceRegInfo = new DeviceRegResponse
                    {
                        DeviceName = deviceName,
                        Password = password,
                        Succeed = true,
                        ErrMsg = string.Empty
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
                    ErrMsg = addDeviceResponse.message
                };
            }
        }

        /// <summary>
        /// 更新设备在线状态
        /// </summary>
        /// <param name="deviceName"></param>
        /// <param name="onlineStatus"></param>
        /// <returns></returns>
        public async Task UpdateDeviceOnlineStatusAsync(string deviceName, OnLineStates onlineStatus)
        {
            var device = await _ioTDbContext.IoTDeviceInfo.Include(o => o.IoTDeviceExtend).AsNoTracking()
                .FirstOrDefaultAsync(o => o.DeviceName == deviceName);
            if (device == null)
            {
                return;
            }
            else
            {
                if (device.IoTDeviceExtend == null) //扩展表为空
                {
                    device.IoTDeviceExtend = new IoTDeviceExtend
                    {
                        DeviceInfoId = device.Id,
                        OnLineStates = (int)onlineStatus,
                    };
                    _ioTDbContext.Attach(device.IoTDeviceExtend);

                    _ioTDbContext.Entry(device.IoTDeviceExtend).State = EntityState.Added;
                    _ioTDbContext.Entry(device.IoTDeviceExtend).Property(o => o.OnLineStates).IsModified = true;
                    await _ioTDbContext.SaveChangesAsync();
                }
                if (device.IoTDeviceExtend.OnLineStates != (int)onlineStatus)         //在线状态不一致
                {
                    device.IoTDeviceExtend.OnLineStates = (int)onlineStatus;

                    _ioTDbContext.Attach(device.IoTDeviceExtend);
                    //防止更新其他字段
                    _ioTDbContext.Entry(device.IoTDeviceExtend).State = EntityState.Unchanged;
                    _ioTDbContext.Entry(device.IoTDeviceExtend).Property(o => o.OnLineStates).IsModified = true;
                    await _ioTDbContext.SaveChangesAsync();
                }
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
            var deviceWare = await _ioTDbContext.IoTDevicewares.FirstOrDefaultAsync(o => o.ProductCode == request.ProductCode && o.UUID == request.UUID);

            if (deviceWare == null)
            {
                return null;
            }
            else
            {
                var deviceInfo = await _ioTDbContext.IoTDeviceInfo.FirstAsync(o => o.DeviceName == deviceWare.DeviceName);

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
        /// <param name="supplyNo"></param>
        /// <param name="productCode"></param>
        /// <param name="uuid"></param>
        /// <returns>
        /// 设备Mqtt名称
        /// </returns>
        private async Task<string> GenerateDeviceNameAsync(string supplyNo, string productCode, string uuid)
        {
            var lastDeviceWare = await _ioTDbContext.IoTDevicewares.Where(o => o.ProductCode == productCode).OrderByDescending(o => o.CreationTime).FirstOrDefaultAsync();

            var newDeviceWare = new IoTDevicewares
            {
                Id = Guid.NewGuid(),
                UUID = uuid,
                ProductCode = productCode,
                CreationTime = DateTime.Now
            };

            if (lastDeviceWare != null && lastDeviceWare.DeviceName.StartsWith(supplyNo + DateTime.Today.ToString("yyyyMMdd")))
            {
                newDeviceWare.DeviceName = (long.Parse(lastDeviceWare.DeviceName) + 1).ToString();
            }
            else
            {
                newDeviceWare.DeviceName = supplyNo + DateTime.Today.ToString("yyyyMMdd") + "0001";
            }
            await _ioTDbContext.IoTDevicewares.AddAsync(newDeviceWare);
            await _ioTDbContext.SaveChangesAsync();

            return newDeviceWare.DeviceName;
        }

        /// <summary>
        /// 获取设备列表
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>

        public async Task<PaginatedListBase<DeviceListViewModel>> GetDeviceListBaseAsync(DeviceListOption options)
        {
            var res = await (
                from d in _ioTDbContext.IoTDeviceInfo
                join p in _ioTDbContext.IoTProductInfo
                    on d.ProductInfoId equals p.Id
                    into productInfo
                from p in productInfo.DefaultIfEmpty()
                join de in _ioTDbContext.IoTDeviceExtend
                    on d.Id equals de.DeviceInfoId
                where p.Id.Equals(options.ProductId)

                select new DeviceListViewModel
                {
                    Id = d.Id,
                    DeviceName = d.DeviceName,
                    OnLineStates = (OnLineStates)de.OnLineStates
                }).Distinct().OrderByDescending(x => x.DeviceName).GetPaginatedListAsync(new PaginatedOptions
                {
                    Page = options.PageIndex,
                    PageSize = options.PageSize,
                    Sorting = null
                });

            return res;
        }
    }
}
