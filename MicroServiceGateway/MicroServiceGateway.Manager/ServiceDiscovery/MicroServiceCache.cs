/****************************************************************************
*项目名称：MicroServiceGateway.Manager.Libs
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：MicroServiceGateway.Manager.Libs
*类 名 称：MicroServiceCache
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：yswenli@outlook.com
*创建时间：2020/8/25 17:41:53
*描述：
*=====================================================================
*修改时间：2020/8/25 17:41:53
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using MicroServiceGateway.Data.Redis;
using MicroServiceGateway.Manager.Libs;
using MicroServiceGateway.Model;
using MicroServiceGateway.Routing;
using SAEA.Common;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MicroServiceGateway.Manager.ServiceDiscovery
{
    public delegate void OnChangedHandler(bool isAdd, MicroServiceConfig microServiceConfig);

    /// <summary>
    /// 微服务缓存
    /// </summary>
    public static class MicroServiceCache
    {
        /// <summary>
        /// 微服务缓存
        /// </summary>
        static MicroServiceCache()
        {
            Task.Factory.StartNew(() =>
            {
                UpdateRouteInfo();

                Thread.Sleep(10 * 1000);

            }, TaskCreationOptions.LongRunning);
        }

        /// <summary>
        /// 更新路由信息到网关服务器
        /// </summary>
        static void UpdateRouteInfo()
        {
            try
            {
                var list = MSGNodeOperation.GetList();

                foreach (var msgnode in list)
                {
                    try
                    {
                        var routes = RouteInfoCache.GetRouteInfos().ConvertToList<Consumer.Model.RouteInfo>();

                        var proxry = MSGNodeRPCServiceCache.Get(msgnode.NodeName);

                        if (proxry != null && proxry.IsConnected)
                        {
                            proxry.NodeService.SetRoutes(routes);
                        }
                        else
                        {
#if DEBUG
                            ConsoleHelper.WriteLine("路由通知到网关服务器失败，msgnode.NodeName：" + msgnode.NodeName);
#endif
                        }
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Error("路由通知到网关服务器失败", ex);
                    }
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                ConsoleHelper.WriteLine("从redis中获取网关节点信息失败：" + ex.Message);
#endif
                LogHelper.Error("从redis中获取网关节点信息失败", ex);

            }
        }

        /// <summary>
        /// 更新微服务信息
        /// </summary>
        /// <param name="microService"></param>
        /// <returns></returns>
        public static bool Set(MicroServiceConfig microService)
        {
            try
            {
                MSInfoOperation.Set(microService);

                var ri = microService.ConvertTo<Model.RouteInfo>();

                RouteInfoCache.Add(ri);

                UpdateRouteInfo();
            }
            catch (Exception ex)
            {
                LogHelper.Error("MicroServiceCache.Set", ex, microService);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 调整缓存过期时间
        /// </summary>
        /// <param name="virtualAddress"></param>
        /// <param name="serviceIP"></param>
        /// <param name="servicePort"></param>
        public static void SetOnline(string virtualAddress, string serviceIP, int servicePort)
        {
            try
            {
                if (MSInfoOperation.GetOnline(virtualAddress, serviceIP, servicePort) == false)
                {
                    var microService = MSInfoOperation.Get(virtualAddress, serviceIP, servicePort);

                    if (microService != null)
                    {
                        var ri = microService.ConvertTo<Model.RouteInfo>();

                        RouteInfoCache.Add(ri);

                        UpdateRouteInfo();
                    }
                }
                MSInfoOperation.SetOnline(virtualAddress, serviceIP, servicePort);

            }
            catch (Exception ex)
            {
                LogHelper.Error("MicroServiceCache.SetOnline", ex, virtualAddress, serviceIP, servicePort);
            }

        }
        /// <summary>
        /// 调整缓存过期时间
        /// </summary>
        /// <param name="virtualAddress"></param>
        /// <param name="serviceIP"></param>
        /// <param name="servicePort"></param>
        /// <returns></returns>
        public static bool GetOnline(string virtualAddress, string serviceIP, int servicePort)
        {
            var online = false;
            try
            {
                online = MSInfoOperation.GetOnline(virtualAddress, serviceIP, servicePort);

                if (!online)
                {
                    var microService = MSInfoOperation.Get(virtualAddress, serviceIP, servicePort);

                    if (microService != null)
                    {
                        RouteInfoCache.Del(serviceIP, servicePort, virtualAddress);

                        UpdateRouteInfo();
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("MicroServiceCache.GetOnline", ex, virtualAddress, serviceIP, servicePort);
            }
            return online;
        }

        /// <summary>
        /// Del
        /// </summary>
        /// <param name="virtualAddress"></param>
        /// <param name="serviceIP"></param>
        /// <param name="servicePort"></param>
        public static bool Del(string virtualAddress, string serviceIP, int servicePort)
        {
            try
            {
                var microService = MSInfoOperation.Get(virtualAddress, serviceIP, servicePort);

                if (microService != null)
                {
                    RouteInfoCache.Del(serviceIP, servicePort, virtualAddress);

                    UpdateRouteInfo();
                }

                return MSInfoOperation.Del(virtualAddress, serviceIP, servicePort);
            }
            catch (Exception ex)
            {
                LogHelper.Error("MicroServiceCache.Del", ex, virtualAddress, serviceIP, servicePort);
            }
            return false;
        }

        /// <summary>
        /// 获取注册的微服务列表
        /// </summary>
        /// <param name="virtualAddress"></param>
        /// <returns></returns>
        public static IEnumerable<MicroServiceConfig> GetList(string virtualAddress)
        {
            return MSInfoOperation.GetList(virtualAddress);
        }

        /// <summary>
        /// 获取VirualAddress
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<string> GetVirualAddress()
        {
            return MSInfoOperation.GetVirtualAddress();
        }
    }
}
