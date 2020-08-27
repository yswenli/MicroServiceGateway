/****************************************************************************
*项目名称：MicroServiceGateway.Routing
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：MicroServiceGateway.Routing
*类 名 称：Router
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：yswenli@outlook.com
*创建时间：2020/8/25 10:14:46
*描述：
*=====================================================================
*修改时间：2020/8/25 10:14:46
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using MicroServiceGateway.Common;
using MicroServiceGateway.Model;
using SAEA.Http.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MicroServiceGateway.Routing
{
    /// <summary>
    /// Router
    /// </summary>
    public class Router
    {
        /// <summary>
        /// 路由
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public static List<RouteInfo> Route(IHttpContext httpContext)
        {
            try
            {
                var tuple = httpContext.Request.RelativeUrl.ToVirtualAddressUrl();

                if (tuple == null)
                {
                    throw new Exception("The URL format is incorrect");
                }

                List<RouteInfo> routeInfos = NodeRouteInfoCache.GetRouteInfos(tuple.Item1);

                if (routeInfos == null || !routeInfos.Any()) throw new Exception("Routing information not found");

                return routeInfos;
            }
            catch (Exception ex)
            {
                Logger.Error("MicroServiceGateway url not found,url:" + httpContext.Request.RelativeUrl, ex);
                httpContext.Response.Status = System.Net.HttpStatusCode.NotFound;
                httpContext.Response.Write("MicroServiceGateway url not found,url:" + httpContext.Request.RelativeUrl);
                httpContext.Response.End();
            }

            return null;
        }
    }
}
