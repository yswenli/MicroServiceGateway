/****************************************************************************
*项目名称：MicroServiceGateway.LoadBalancer
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：MicroServiceGateway.LoadBalancer
*类 名 称：LoadBalance
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：yswenli@outlook.com
*创建时间：2020/8/25 11:10:19
*描述：
*=====================================================================
*修改时间：2020/8/25 11:10:19
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using MicroServiceGateway.Common;
using MicroServiceGateway.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MicroServiceGateway.LoadBalancer
{
    /// <summary>
    /// 负载均衡
    /// </summary>
    public static class LoadBalance
    {
        static Random _random;

        /// <summary>
        /// 负载均衡
        /// </summary>
        static LoadBalance()
        {
            _random = new Random(Environment.TickCount);
        }

        /// <summary>
        /// 根据微服务配置的权重来随机获取路由
        /// </summary>
        /// <param name="routeInfos"></param>
        /// <returns></returns>
        public static RouteInfo Get(List<RouteInfo> routeInfos)
        {
            try
            {
                if (routeInfos != null && routeInfos.Any())
                {
                    var scores = routeInfos.Sum(b => b.Score);

                    var next = _random.Next(0, scores);

                    var score = 0;

                    for (int i = 0; i < routeInfos.Count; i++)
                    {
                        var cur = routeInfos[i];

                        score += routeInfos[i].Score;

                        if (next <= score)

                            return cur;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error("LoadBalance.Get error", ex);
            }
            return null;
        }
    }
}
