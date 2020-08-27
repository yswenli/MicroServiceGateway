/****************************************************************************
*项目名称：MicroServiceGateway.Routing
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：MicroServiceGateway.Routing
*类 名 称：NodeRouteInfoCache
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：yswenli@outlook.com
*创建时间：2020/8/24 17:42:05
*描述：
*=====================================================================
*修改时间：2020/8/24 17:42:05
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using MicroServiceGateway.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MicroServiceGateway.Routing
{
    /// <summary>
    /// 网关节点路由缓存
    /// </summary>
    public static class NodeRouteInfoCache
    {
        static RouteTable _routeTable;

        /// <summary>
        /// 网关节点路由缓存
        /// </summary>
        static NodeRouteInfoCache()
        {
            _routeTable = new RouteTable();
        }

        /// <summary>
        /// 更新本地缓存
        /// </summary>
        /// <param name="routeInfos"></param>
        public static void Set(List<RouteInfo> routeInfos)
        {
            _routeTable.Set(routeInfos);
        }

        /// <summary>
        /// 根据虚拟地扯获取微服务信息
        /// </summary>
        /// <param name="virtualAddress"></param>
        /// <returns></returns>
        public static List<RouteInfo> GetRouteInfos(string virtualAddress)
        {
            var services = _routeTable.GetServices(virtualAddress);

            if (services == null || !services.Any())
            {
                throw new Exception("MicroServiceGateway virtualAddress not found,virtualAddress:" + virtualAddress);
            }

            return services;
        }
    }
}
