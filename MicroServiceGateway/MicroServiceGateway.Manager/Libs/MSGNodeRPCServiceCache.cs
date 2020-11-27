using MicroServiceGateway.Data.Redis;
using MicroServiceGateway.Manager.Consumer;
using MicroServiceGateway.Model;
using SAEA.Common;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MicroServiceGateway.Manager.Libs
{
    /// <summary>
    /// 微服务节点rpc服务缓存
    /// </summary>
    public static class MSGNodeRPCServiceCache
    {
        static ConcurrentDictionary<string, RPCServiceProxy> _dic;

        static ConcurrentDictionary<string, PerformaceModel> _cache;

        /// <summary>
        /// 微服务节点rpc服务缓存
        /// </summary>
        static MSGNodeRPCServiceCache()
        {
            _dic = new ConcurrentDictionary<string, RPCServiceProxy>();
            _cache = new ConcurrentDictionary<string, PerformaceModel>();
        }

        /// <summary>
        /// Set
        /// </summary>
        /// <param name="name"></param>
        /// <param name="serviceProxy"></param>
        public static void Set(string name, RPCServiceProxy serviceProxy)
        {
            _dic.AddOrUpdate(name, serviceProxy, (k, v) => serviceProxy);
        }

        /// <summary>
        /// Get
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static RPCServiceProxy Get(string name)
        {
            if (_dic.TryGetValue(name, out RPCServiceProxy serviceProxy))
            {
                return serviceProxy;
            }
            return null;
        }

        /// <summary>
        /// Del
        /// </summary>
        /// <param name="name"></param>
        public static void Del(string name)
        {
            if (_dic.TryRemove(name, out RPCServiceProxy serviceProxy))
            {
                _cache.TryRemove(name, out PerformaceModel val);

                serviceProxy.Dispose();
            }
        }

        /// <summary>
        /// GetPerformace
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static PerformaceModel GetPerformace(string name)
        {
            if (_cache.TryGetValue(name, out PerformaceModel performaceModel))
            {
                if (performaceModel != null)
                {
                    return performaceModel;
                }
            }
            return new PerformaceModel();
        }


        static bool _isStarted = false;

        /// <summary>
        /// Start
        /// </summary>
        public static void Start()
        {
            if (!_isStarted)
            {
                _isStarted = true;

                Task.Factory.StartNew(() =>
                {
                    var stopwatch = Stopwatch.StartNew();

                    while (_isStarted)
                    {
                        UpdateMsgNodeInfos(stopwatch);
                    }
                    stopwatch.Stop();
                }, TaskCreationOptions.LongRunning);
            }
        }

        static void UpdateMsgNodeInfos(Stopwatch stopwatch)
        {
            try
            {
                if (!_dic.IsEmpty)
                {
                    foreach (var item in _dic)
                    {
                        try
                        {
                            var p = item.Value.NodeService.GetPerformace();
                            var cp = p.ConvertTo<PerformaceModel>();
                            _cache.AddOrUpdate(item.Key, cp, (k, v) => cp);

                            var nodeInfo = MSGNodeOperation.Get(item.Key);
                            if (nodeInfo != null)
                            {
                                nodeInfo.Linked = true;
                                nodeInfo.LinkedTime = DateTimeHelper.Now;
                                MSGNodeOperation.Set(nodeInfo);
                            }
                        }
                        catch (Exception ex)
                        {
                            LogHelper.Error("MSGNodeRPCServiceDic.Start 获取结点详情失败", ex, item);
                        }
                    }
                }
                else
                {
                    try
                    {
                        var list = MSGNodeOperation.GetList();

                        foreach (var msgnode in list)
                        {
                            try
                            {
                                RPCServiceProxy serviceProxy = new RPCServiceProxy($"rpc://{msgnode.NodeIP}:{msgnode.NodeRpcPort}");

                                var performace = serviceProxy.NodeService.GetPerformace();

#if DEBUG
                                Console.WriteLine($"已连接到网关 rpc://{msgnode.NodeIP}:{msgnode.NodeRpcPort}");
#endif

                                if (performace != null)
                                {
                                    msgnode.Linked = true;
                                    msgnode.LinkedTime = DateTime.Now;

                                    Set(msgnode.NodeName, serviceProxy);

                                    MSGNodeOperation.Set(msgnode);
                                }
                            }
                            catch (Exception ex)
                            {
                                LogHelper.Error("MSGNodeRPCServiceDic.Start foreach", ex);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Error("MSGNodeRPCServiceDic.Start", ex);
                    }
                }
            }
            finally
            {
                var remain = 1000 - (int)stopwatch.ElapsedMilliseconds;
                if (remain > 0)
                {
                    ThreadHelper.Sleep(remain);
                }
                stopwatch.Restart();
            }
        }

        /// <summary>
        /// Stop
        /// </summary>
        public static void Stop()
        {
            _isStarted = false;
        }
    }
}
