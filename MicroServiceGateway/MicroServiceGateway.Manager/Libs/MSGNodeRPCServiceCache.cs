using MicroServiceGateway.Data.Redis;
using MicroServiceGateway.Manager.Consumer;
using MicroServiceGateway.Model;
using SAEA.Common;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MicroServiceGateway.Manager.Libs
{
    /// <summary>
    /// 微服务节点rpc服务缓存
    /// </summary>
    public static class MsgNodeRpcServiceCache
    {
        static ConcurrentDictionary<string, RPCServiceProxy> _msgNodeRpcServiceProxyCache;

        /// <summary>
        /// 微服务节点rpc服务缓存
        /// </summary>
        static MsgNodeRpcServiceCache()
        {
            _msgNodeRpcServiceProxyCache = new ConcurrentDictionary<string, RPCServiceProxy>();
        }

        /// <summary>
        /// Set
        /// </summary>
        /// <param name="nodeName"></param>
        /// <param name="serviceProxy"></param>
        public static void Set(string nodeName, RPCServiceProxy serviceProxy)
        {
            _msgNodeRpcServiceProxyCache.AddOrUpdate(nodeName, serviceProxy, (k, v) =>
            {
                v.Dispose();
                return serviceProxy;
            });
        }

        /// <summary>
        /// Get
        /// </summary>
        /// <param name="nodeName"></param>
        /// <returns></returns>
        public static RPCServiceProxy Get(string nodeName)
        {
            if (_msgNodeRpcServiceProxyCache.TryGetValue(nodeName, out RPCServiceProxy serviceProxy))
            {
                return serviceProxy;
            }
            return null;
        }

        /// <summary>
        /// getlist
        /// </summary>
        /// <returns></returns>
        public static List<RPCServiceProxy> GetList()
        {
            if (!_msgNodeRpcServiceProxyCache.IsEmpty)
            {
                return _msgNodeRpcServiceProxyCache.Values.ToList();
            }
            return null;
        }

        /// <summary>
        /// Del
        /// </summary>
        /// <param name="nodeName"></param>
        public static void Del(string nodeName)
        {
            if (_msgNodeRpcServiceProxyCache.TryRemove(nodeName, out RPCServiceProxy serviceProxy))
            {
                MsgNodePerformanceCache.Del(nodeName);

                serviceProxy.Dispose();
            }
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
                if (!_msgNodeRpcServiceProxyCache.IsEmpty)
                {
                    foreach (var item in _msgNodeRpcServiceProxyCache)
                    {
                        try
                        {
                            var p = item.Value.NodeService.GetPerformace();

                            var cp = p.ConvertTo<PerformaceModel>();

                            MsgNodePerformanceCache.Set(item.Key, cp);

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
                                ConsoleHelper.WriteLine($"已成功连接到网关 rpc://{msgnode.NodeIP}:{msgnode.NodeRpcPort}");
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
