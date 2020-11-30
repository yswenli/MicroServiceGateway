﻿using MicroServiceGateway.Manager.Consumer;
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

        static ConcurrentDictionary<string, long> _apiCountCache = new ConcurrentDictionary<string, long>();

        /// <summary>
        /// ApiCallLogCache
        /// </summary>
        static ApiCallLogCache()
        {
            Task.Factory.StartNew(() =>
            {
                var date = DateTimeHelper.Now;

                while (true)
                {
                    if (DateTimeHelper.Now.Date > date)
                    {
                        date = DateTimeHelper.Now.Date;

                        _apiCountCache.Clear();
                    }
                }
            }, TaskCreationOptions.LongRunning);
        }

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
                        _apiCountCache.AddOrUpdate(item.Url, item.Count, (k, v) => { v += item.Count; return v; });
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



        public static List<Apistatistical> GetApistatisticals()
        {

        }
    }
}
