/****************************************************************************
*项目名称：MicroServiceGateway.Routing
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：MicroServiceGateway.Routing
*类 名 称：RouteTable
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：yswenli@outlook.com
*创建时间：2020/8/22 17:54:29
*描述：
*=====================================================================
*修改时间：2020/8/22 17:54:29
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using MicroServiceGateway.Model;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MicroServiceGateway.Routing
{
    /// <summary>
    /// 路由表
    /// </summary>
    public class RouteTable : IEnumerable<RouteInfo>
    {
        List<RouteInfo> _routeInfos;

        object _locker = new object();

        /// <summary>
        /// GetEnumerator
        /// </summary>
        /// <returns></returns>
        public IEnumerator<RouteInfo> GetEnumerator()
        {
            return _routeInfos.GetEnumerator();
        }

        /// <summary>
        /// GetEnumerator
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// 路由表
        /// </summary>
        public RouteTable()
        {
            _routeInfos = new List<RouteInfo>();
        }

        /// <summary>
        /// 路由表
        /// </summary>
        /// <param name="cluster"></param>
        /// <returns></returns>
        public List<RouteInfo> this[string cluster]
        {
            get
            {
                lock (_locker)
                {
                    return _routeInfos.Where(b => b.Cluster == cluster).ToList();
                }
            }
        }

        /// <summary>
        /// 获取某集群下的全部服务
        /// </summary>
        /// <param name="cluster"></param>
        /// <returns></returns>
        public List<RouteInfo> GetServices(string cluster)
        {
            lock (_locker)
            {
                return _routeInfos.Where(b => b.Cluster == cluster).ToList();
            }
        }

        /// <summary>
        /// 获取某集群某环境下的全部服务
        /// </summary>
        /// <param name="cluster"></param>
        /// <param name="env"></param>
        /// <returns></returns>
        public List<RouteInfo> GetServices(string cluster, string env)
        {
            lock (_locker)
            {
                return _routeInfos.Where(b => b.Cluster == cluster && b.Env == env).ToList();
            }
        }
        /// <summary>
        /// 获取某集群某环境某服务下的全部网关节点
        /// </summary>
        /// <param name="cluster"></param>
        /// <param name="env"></param>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public List<RouteInfo> GetServices(string cluster, string env, string serviceName)
        {
            lock (_locker)
            {
                return _routeInfos.Where(b => b.Cluster == cluster && b.Env == env && b.ServiceName == serviceName).ToList();
            }
        }

        /// <summary>
        /// 根据微服务ip和port获取列表
        /// </summary>
        /// <param name="serviceIP"></param>
        /// <param name="servicePort"></param>
        /// <param name="virtualAddress"></param>
        /// <returns></returns>
        public List<RouteInfo> Get(string serviceIP, int servicePort, string virtualAddress)
        {
            lock (_locker)
            {
                return _routeInfos.Where(b => b.ServiceIP == serviceIP && b.ServicePort == servicePort && b.VirtualAddress == virtualAddress).ToList();
            }
        }

        /// <summary>
        /// 添加节点
        /// </summary>
        /// <param name="routeInfo"></param>
        public void Set(RouteInfo routeInfo)
        {
            lock (_locker)
            {
                var rt = _routeInfos.Where(b => b.ServiceIP == routeInfo.ServiceIP && b.ServicePort == routeInfo.ServicePort && b.VirtualAddress == routeInfo.VirtualAddress).FirstOrDefault();

                if (rt != null)
                {
                    _routeInfos.Remove(rt);
                }
                _routeInfos.Add(routeInfo);
            }
        }

        /// <summary>
        /// 更新本机缓存
        /// </summary>
        /// <param name="routeInfos"></param>
        public void Set(List<RouteInfo> routeInfos)
        {
            lock (_locker)
            {
                _routeInfos = routeInfos;
            }
        }

        /// <summary>
        /// 不存在则添加
        /// </summary>
        /// <param name="routeInfo"></param>
        public void Add(RouteInfo routeInfo)
        {
            lock (_locker)
            {
                var rt = _routeInfos.Where(b => b.ServiceIP == routeInfo.ServiceIP && b.ServicePort == routeInfo.ServicePort && b.VirtualAddress == routeInfo.VirtualAddress).FirstOrDefault();

                if (rt == null)
                {
                    _routeInfos.Add(routeInfo);
                }
            }
        }
    }
}
