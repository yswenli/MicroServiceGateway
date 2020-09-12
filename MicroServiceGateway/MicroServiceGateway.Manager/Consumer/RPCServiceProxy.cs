/*******
* 此代码为SAEA.RPC.Generater生成
* 尽量不要修改此代码 2020-09-12 17:22:20
*******/

using System;
using System.Collections.Generic;
using SAEA.Common;
using SAEA.RPC.Consumer;
using SAEA.RPC.Model;
using MicroServiceGateway.Manager.Consumer.Model;
using MicroServiceGateway.Manager.Consumer.Service;

namespace MicroServiceGateway.Manager.Consumer
{
    public class RPCServiceProxy
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
            _No = new NodeService(_serviceConsumer);
        }
        private void ExceptionCollector_OnErr(string name, Exception ex)
        {
            OnErr?.Invoke(name, ex);
        }
        private void _serviceConsumer_OnNoticed(byte[] serializeData)
        {
            OnNoticed?.Invoke(serializeData);
        }
        NodeService _No;
        public NodeService NodeService
        {
             get{ return _No; }
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

namespace MicroServiceGateway.Manager.Consumer.Service
{
    public class NodeService
    {
        ServiceConsumer _serviceConsumer;
        public NodeService(ServiceConsumer serviceConsumer)
        {
            _serviceConsumer = serviceConsumer;
        }
        public String Ping()
        {
            return _serviceConsumer.RemoteCall<String>("NodeService", "Ping");
        }
        public bool SetRoutes(List<RouteInfo> routeInfos)
        {
            return _serviceConsumer.RemoteCall<bool>("NodeService", "SetRoutes", routeInfos);
        }
        public PerformaceModel GetPerformace()
        {
            return _serviceConsumer.RemoteCall<PerformaceModel>("NodeService", "GetPerformace");
        }
    }
}

namespace MicroServiceGateway.Manager.Consumer.Model
{
    public class RouteInfo
    {
        public Int32 Score
        {
            get;set;
        }
        public Int32 Timeout
        {
            get;set;
        }
        public Boolean Fused
        {
            get;set;
        }
        public DateTime FuseTime
        {
            get;set;
        }
        public String VirtualAddress
        {
            get;set;
        }
        public String ServiceName
        {
            get;set;
        }
        public String ServiceIP
        {
            get;set;
        }
        public Int32 ServicePort
        {
            get;set;
        }
        public String Description
        {
            get;set;
        }
    }
}

namespace MicroServiceGateway.Manager.Consumer.Model
{
    public class PerformaceModel
    {
        public String VirtualAddress
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
        public DateTime Created
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

