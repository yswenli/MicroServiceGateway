/****************************************************************************
*项目名称：MicroServiceGateway.Manager.Services
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：MicroServiceGateway.Routing
*类 名 称：RouteInfoCache
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：yswenli@outlook.com
*创建时间：2020/8/23 14:58:31
*描述：
*=====================================================================
*修改时间：2020/8/23 14:58:31
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using MicroServiceGateway.Common;
using MicroServiceGateway.Data.Redis;
using MicroServiceGateway.Model;
using System.Collections.Generic;
using System.Linq;

namespace MicroServiceGateway.Routing
{
    /// <summary>
    /// 节点管理路由信息缓存
    /// </summary>
    public static class RouteInfoCache
    {
        static RouteTable _routeTable;

        /// <summary>
        /// 路由信息缓存
        /// </summary>
        static RouteInfoCache()
        {
            _routeTable = new RouteTable();
            var ris = MSGRouteInfoOperation.Read();
            _routeTable.Set(ris);
        }

        /// <summary>
        /// 获取routeinfos
        /// </summary>
        /// <returns></returns>
        public static List<RouteInfo> GetRouteInfos()
        {
            return _routeTable.ToList();
        }

        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="routeInfo"></param>
        public static void Set(RouteInfo routeInfo)
        {
            _routeTable.Set(routeInfo);
            MSGRouteInfoOperation.Write(_routeTable.ToList());
        }
        /// <summary>
        /// add
        /// </summary>
        /// <param name="routeInfo"></param>
        public static void Add(RouteInfo routeInfo)
        {
            _routeTable.Add(routeInfo);
            MSGRouteInfoOperation.Write(_routeTable.ToList());
        }
        /// <summary>
        /// Del
        /// </summary>
        /// <param name="serviceIP"></param>
        /// <param name="servicePort"></param>
        /// <param name="virtualAddress"></param>
        public static void Del(string serviceIP, int servicePort, string virtualAddress)
        {
            _routeTable.Del(serviceIP, servicePort, virtualAddress);
            MSGRouteInfoOperation.Write(_routeTable.ToList());
        }
    }
}
