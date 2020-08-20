/****************************************************************************
*项目名称：MicroServiceGateway.Client
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：MicroServiceGateway.Client
*类 名 称：MSGMiddleware
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：yswenli@outlook.com
*创建时间：2020/8/20 13:54:01
*描述：
*=====================================================================
*修改时间：2020/8/20 13:54:01
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using MicroServiceGateway.Client.Consumer;
using MicroServiceGateway.Common;
using MicroServiceGateway.Model;
using Microsoft.AspNetCore.Http;
using SAEA.Common;
using System;
using System.Threading.Tasks;

namespace MicroServiceGateway.Client
{
    /// <summary>
    /// 微服务客户中间件
    /// </summary>
    public class MSGMiddleware
    {
        private readonly RequestDelegate _next;

        MicroServiceConfig _microServiceConfig = null;

        RPCServiceProxy _rpcServiceProxy;

        /// <summary>
        /// 微服务客户asp.net core中间件
        /// </summary>
        /// <param name="next"></param>
        public MSGMiddleware(RequestDelegate next)
        {
            _next = next;

            try
            {
                if (_microServiceConfig == null)
                {
                    _microServiceConfig = MicroServiceConfig.Read();

                    var errStr = _microServiceConfig.CheckNull();

                    if (!string.IsNullOrEmpty(errStr))

                        throw new Exception($"在MicroServiceConfig配置中{errStr}");

                    _rpcServiceProxy = new RPCServiceProxy($"rpc://{_microServiceConfig.ManagerServerIP}:{_microServiceConfig.ManagerServerPort}/");

                    _rpcServiceProxy.MSGClientService.Regist(_microServiceConfig.ConvertTo<Consumer.Model.MicroServiceConfig>());

                    PerformanceHelper.OnCounted += PerformanceHelper_OnCounted;

                    PerformanceHelper.Start();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("初始化MicroServiceConfig配置失败，请检查MicroServiceConfig配置文件及其内容是否正确", ex);
            }
        }

        private void PerformanceHelper_OnCounted(Performace performace)
        {
            if (_rpcServiceProxy != null && _rpcServiceProxy.IsConnected)
            {
                _rpcServiceProxy.MSGClientService.Report(new Consumer.Model.PerformaceModel()
                {
                    IP = _microServiceConfig.IP,
                    Port = _microServiceConfig.Port,
                    ServiceName = _microServiceConfig.ServiceName,
                    CPU = performace.CPU,
                    MemoryUsage = performace.MemoryUsage,
                    TotalThreads = performace.TotalThreads,
                    BytesRec = performace.BytesRec,
                    BytesSen = performace.BytesSen,
                    HandleCount = performace.HandleCount
                });
            }
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await _next(context);
        }
    }
}
