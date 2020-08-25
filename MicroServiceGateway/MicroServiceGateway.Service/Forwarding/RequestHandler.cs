/****************************************************************************
*项目名称：MicroServiceGateway.Service.Forwarding
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：MicroServiceGateway.Service.Forwarding
*类 名 称：RequestHandler
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：yswenli@outlook.com
*创建时间：2020/8/22 11:00:21
*描述：
*=====================================================================
*修改时间：2020/8/22 11:00:21
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using MicroServiceGateway.Common;
using MicroServiceGateway.Model;
using MicroServiceGateway.Routing;
using SAEA.Http;
using SAEA.Http.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;

namespace MicroServiceGateway.Service.Forwarding
{
    /// <summary>
    /// webhost请求处理
    /// </summary>
    public class RequestHandler
    {
        Stopwatch _stopwatch;

        /// <summary>
        /// webhost请求处理
        /// </summary>
        /// <param name="httpContext"></param>
        public RequestHandler()
        {
            _stopwatch = new Stopwatch();
        }

        /// <summary>
        /// 处理
        /// </summary>
        public void Invoke(IHttpContext httpContext)
        {
            _stopwatch.Start();


            _stopwatch.Stop();
        }

        static List<RouteInfo> Route(IHttpContext httpContext)
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

        static RouteInfo LoadBalance(IHttpContext httpContext, List<RouteInfo> routeInfos)
        {
            try
            {

                //todo
            }
            catch (Exception ex)
            {

            }
        }

        static HttpResponseMessage Forward(IHttpContext httpContext, RouteInfo routeInfo)
        {
            HttpResponseMessage result = null;

            try
            {
                HttpMethod httpMethod = new HttpMethod(httpContext.Request.Method);

                var msg = new HttpRequestMessage()
                {
                    Method = httpMethod
                };
                foreach (var item in httpContext.Request.Headers)
                {
                    msg.Headers.Add(item.Key, item.Value);
                }
                var httpClient = HttpClientWrapper.GetWrapper(routeInfo.VirtualAddress, routeInfo.ServiceIP, routeInfo.ServicePort);

                using (CancellationTokenSource cts = new CancellationTokenSource())
                {
                    var task = httpClient.SendAsync(msg, cts.Token);
                    task.Wait();
                    result = task.Result;
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }


    }
}
