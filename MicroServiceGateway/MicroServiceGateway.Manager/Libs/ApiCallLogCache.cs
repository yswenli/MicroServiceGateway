using MicroServiceGateway.Data.Redis;
using MicroServiceGateway.Manager.Consumer;
using MicroServiceGateway.Manager.Consumer.Model;
using SAEA.Common;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MicroServiceGateway.Manager.Libs
{
    /// <summary>
    /// ApiCallLogCache
    /// </summary>
    public static class ApiCallLogCache
    {
        static bool _running = false;

        public static void Start()
        {
            if (!_running)
            {
                _running = true;

                Task.Factory.StartNew(() =>
                {
                    while (_running)
                    {
                        Thread.Sleep(1000);

                        var nodes = MsgNodeRpcServiceCache.GetList();

                        if (nodes != null && nodes.Any())
                        {
                            foreach (var item in nodes)
                            {
                                GetApiCount(item);
                                GetApiLogs(item);
                            }
                        }
                    }
                }, TaskCreationOptions.LongRunning);
            }
        }


        static void GetApiCount(RPCServiceProxy serviceProxy)
        {
            try
            {
                var apiStatics = serviceProxy.NodeService.GetApiStatics();

                if (apiStatics != null)
                {
                    foreach (var item in apiStatics)
                    {
                        ApiCounterOperation.Set(item.Url, item.Count);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("ApiCallLogCache.GetApiCount", ex, serviceProxy);
            }
        }



        static void GetApiLogs(RPCServiceProxy serviceProxy)
        {
            try
            {
                var logs = serviceProxy.NodeService.GetApiLogs();

                var fileName = PathHelper.Combine(PathHelper.GetCurrentPath(), "apilogs.log");

                using (var fs = File.Open(fileName, FileMode.Append, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    using (var sw = new StreamWriter(fs, Encoding.UTF8))
                    {
                        sw.Write(string.Join(Environment.NewLine, logs));
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("ApiCallLogCache.GetApiLogsAsync", ex);
            }
        }



        public static void Stop()
        {
            _running = false;
        }


        /// <summary>
        /// get api访问计数器
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public static List<MicroServiceGateway.Model.Apistatistical> GetApistatisticals(int pageIndex)
        {
            if (pageIndex < 0) pageIndex = 1;

            var count = 12;

            var result= ApiCounterOperation.GetApistatisticals();

            if (result.Any())
            {
                result = result.OrderByDescending(b => b.Count).Skip((pageIndex - 1) * count).Take(count).ToList();
            }
            return result;
        }
    }
}
