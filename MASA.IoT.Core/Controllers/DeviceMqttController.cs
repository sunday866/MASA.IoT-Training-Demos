﻿using Dapr;
using MASA.IoT.Common;
using MASA.IoT.Core.Contract.Enum;
using MASA.IoT.Core.Contract.Measurement;
using MASA.IoT.Core.Contract.Mqtt;
using MASA.IoT.Core.GateWay;
using MASA.IoT.Core.IHandler;
using Microsoft.AspNetCore.Mvc;

namespace MASA.IoT.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceMqttController : ControllerBase
    {
        private IDeviceHandler _deviceHandler;
        private IRulesEngineGateWay _rulesEngineGateWay;

        public DeviceMqttController(IDeviceHandler deviceHandler, IRulesEngineGateWay rulesEngineGateWay)
        {
            _deviceHandler = deviceHandler;
            _rulesEngineGateWay = rulesEngineGateWay;
        }

        [Topic("pubsub", "DeviceMessage")]
        [HttpPost("DeviceMessage")]
        public async Task DeviceMessageAsync([FromBody] PubSubOptions pubSubOptions)
        {
            await _rulesEngineGateWay.SendDataAsync(pubSubOptions);
            await _deviceHandler.WriteMeasurementAsync<PubSubOptions>(pubSubOptions);
            Console.WriteLine($"Subscriber received, DeviceName:{pubSubOptions.DeviceName},Msg:{pubSubOptions.Msg}");
        }



        [HttpPost("WriteTestData")]
        public async Task WriteTestDataAsync()
        {
            await _deviceHandler.WriteTestDataAsync();
            //Console.WriteLine($"Subscriber received, DeviceName:{pubSubOptions.DeviceName},Msg:{pubSubOptions.Msg}");
        }


        /// <summary>
        /// 设备响应RPC请求
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("RespondToRpc")]
        public async Task<bool> RespondToRpcAsync(RespondRpcMessageRequest request)
        {
            var infoArr = request.Topic.Split("/");
            var result = _deviceHandler.RespondToRpc(new RpcMessageRequest
            {
                DeviceName = infoArr[2],
                RequestId = Guid.Parse(infoArr[3]),
                ProductId = Guid.Parse(infoArr[1]),
                MessageType = MessageType.Up,
                MessageId = request.MessageId,
                MessageData = request.Payload,
            });
            return await Task.FromResult(result);
        }

        /// <summary>
        /// 发送RPC式调用，并同步等待设备返回
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("SendRpcMessage")]
        public async Task<RpcMessageResponse> SendRpcMessageAsync(SendRpcMessageRequest request)
        {
            return await _deviceHandler.PublishAndGetResponseAsync(new RpcMessageRequest
            {
                DeviceName = request.DeviceName,
                RequestId = Guid.NewGuid(),
                ProductId = request.ProductId,
                MessageType = MessageType.Up,
                MessageData = request.MessageData,
                Timeout = request.Timeout
            });
        }
    }
}
