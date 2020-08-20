/*******
* 此代码为SAEA.RPC.Generater生成
* 尽量不要修改此代码 2020-08-20 15:26:42
*******/

using MicroServiceGateway.Client.Consumer.Model;
using MicroServiceGateway.Client.Consumer.Service;
using SAEA.Common;
using SAEA.RPC.Consumer;
using SAEA.RPC.Model;
using System;

namespace MicroServiceGateway.Client.Consumer
{
    class RPCServiceProxy
    {
        public event ExceptionCollector.OnErrHander OnErr;

        public event OnNoticedHandler OnNoticed;

        ServiceConsumer _serviceConsumer;

        public bool IsConnected
        {
            get{ return _serviceConsumer.IsConnected; }
        }
        public RPCServiceProxy(string uri = "rpc://127.0.0.1:39654") : this(uri, 4, 5, 10 * 1000) { }
        public RPCServiceProxy(string uri, int links = 4, int retry = 5, int timeOut = 10 * 1000)
        {
            ExceptionCollector.OnErr += ExceptionCollector_OnErr;
            _serviceConsumer = new ServiceConsumer(new Uri(uri), links, retry, timeOut);
            _serviceConsumer.OnNoticed += _serviceConsumer_OnNoticed;
            _MS = new MSGClientService(_serviceConsumer);
        }
        private void ExceptionCollector_OnErr(string name, Exception ex)
        {
            OnErr?.Invoke(name, ex);
        }
        private void _serviceConsumer_OnNoticed(byte[] serializeData)
        {
            OnNoticed?.Invoke(serializeData);
        }
        MSGClientService _MS;
        public MSGClientService MSGClientService
        {
             get{ return _MS; }
        }
        public void RegistReceiveNotice()
        {
            _serviceConsumer.RegistReceiveNotice();
        }
        public void Dispose()
        {
            _serviceConsumer.Dispose();
        }
    }
}

namespace MicroServiceGateway.Client.Consumer.Service
{
    public class MSGClientService
    {
        ServiceConsumer _serviceConsumer;
        public MSGClientService(ServiceConsumer serviceConsumer)
        {
            _serviceConsumer = serviceConsumer;
        }
        public Boolean Regist(MicroServiceConfig microServiceConfig)
        {
            return _serviceConsumer.RemoteCall<Boolean>("MSGClientService", "Regist", microServiceConfig);
        }
        public Boolean Report(PerformaceModel performaceModel)
        {
            return _serviceConsumer.RemoteCall<Boolean>("MSGClientService", "Report", performaceModel);
        }
    }
}

namespace MicroServiceGateway.Client.Consumer.Model
{
    public class MicroServiceConfig
    {
        /// <summary>
        /// 微服务管理地址
        /// </summary>
        public string ManagerServerIP { get; set; }
        /// <summary>
        /// 微服务管理端口
        /// </summary>
        public int ManagerServerPort { get; set; }

        /// <summary>
        /// 网关转发地址
        /// </summary>
        public string VirtualAddress { get; set; }

        /// <summary>
        /// 服务环境
        /// </summary>
        public string Env { get; set; } = "dev";
        /// <summary>
        /// 服务名称
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// 服务ip
        /// </summary>
        public string IP { get; set; }
        /// <summary>
        /// 服务端口
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 服务健康检测地址
        /// </summary>
        public string HealthChecksUrl { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
    }
}

namespace MicroServiceGateway.Client.Consumer.Model
{
    public class PerformaceModel
    {
        public String ServiceName
        {
            get;set;
        }
        public String IP
        {
            get;set;
        }
        public Int32 Port
        {
            get;set;
        }
        public Single CPU
        {
            get;set;
        }
        public Single MemoryUsage
        {
            get;set;
        }
        public Single TotalThreads
        {
            get;set;
        }
        public Single HandleCount
        {
            get;set;
        }
        public Single BytesRec
        {
            get;set;
        }
        public Single BytesSen
        {
            get;set;
        }
    }
}

