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
using MicroServiceGateway.Calllogger;
using MicroServiceGateway.Common;
using MicroServiceGateway.Model;
using MicroServiceGateway.Routing;
using MicroServiceGateway.Service.Common;
using SAEA.Common;
using SAEA.RPC;
using System;
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

        /// <summary>
        /// 网关节点服务
        /// </summary>
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
#if DEBUG
            ConsoleHelper.WriteLine("收到路由信息：" + SerializeHelper.Serialize(routeInfos));
#endif
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


        /// <summary>
        /// 读取调用日志
        /// </summary>
        /// <returns></returns>
        public List<string> GetApiLogs()
        {
            return CallLog.GetApiLogs();
        }


        /// <summary>
        /// 读取调用次数统计
        /// </summary>
        /// <returns></returns>
        public ApiStatics GetApiStatics()
        {
            return CallLog.GetApiStatics();
        }
    }
}
