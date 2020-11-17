/****************************************************************************
*项目名称：MicroServiceGateway.Service.Services
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：MicroServiceGateway.Service.Services
*类 名 称：NodeService
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：yswenli@outlook.com
*创建时间：2020/8/27 10:59:46
*描述：
*=====================================================================
*修改时间：2020/8/27 10:59:46
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using MicroServiceGateway.Model;
using MicroServiceGateway.Routing;
using MicroServiceGateway.Service.Common;
using SAEA.RPC;
using System.Collections.Generic;

namespace MicroServiceGateway.Service.Services
{
    /// <summary>
    /// 网关节点服务
    /// </summary>
    [RPCService]
    public class NodeService
    {
        static MSGNodeConfig _msgnodeConfig;
        static NodeService()
        {
            _msgnodeConfig = MSGNodeConfig.Read();
        }

        /// <summary>
        /// 设置路由
        /// </summary>
        /// <param name="routeInfos"></param>
        /// <returns></returns>
        public bool SetRoutes(List<RouteInfo> routeInfos)
        {
            return NodeRouteInfoCache.Set(routeInfos);
        }

        /// <summary>
        /// 获取资源使用情况
        /// </summary>
        /// <returns></returns>
        public PerformaceModel GetPerformace()
        {
            var performance = StatisticsReporter.PerformaceModel;
            performance.IP = _msgnodeConfig.NodeIP;
            performance.Port = _msgnodeConfig.NodeRpcPort;
            return performance;
        }

    }
}
