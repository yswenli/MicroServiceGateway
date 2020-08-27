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
using MicroServiceGateway.Calllogger;
using MicroServiceGateway.Common;
using MicroServiceGateway.LoadBalancer;
using MicroServiceGateway.Model;
using MicroServiceGateway.Routing;
using SAEA.Common;
using SAEA.Http.Model;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;

namespace MicroServiceGateway.Service.Forwarding
{
    /// <summary>
    /// webhost请求处理
    /// </summary>
    public class RequestHandler
    {
        /// <summary>
        /// 处理
        /// </summary>
        public void Invoke(IHttpContext httpContext)
        {
            var routes = Router.Route(httpContext);

            if (routes != null)
            {
                var routeInfo = LoadBalance.Get(routes);

                Forward(httpContext, routeInfo);
            }
        }

        /// <summary>
        /// 转发
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="routeInfo"></param>
        /// <returns></returns>
        static async void Forward(IHttpContext httpContext, RouteInfo routeInfo)
        {
            var stopwatch = Stopwatch.StartNew();

            string url = $"http://{routeInfo.ServiceIP}:{routeInfo.ServicePort}{(httpContext.Request.RelativeUrl.Substring(0, 1) == "/" ? httpContext.Request.RelativeUrl : "/" + httpContext.Request.RelativeUrl)}";

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

                msg.RequestUri = new Uri(url);

                if (httpContext.Request.Method == "POST")
                {
                    msg.Content = new ByteArrayContent(httpContext.Request.Body);
                }

                var httpClient = HttpClientWrapper.GetWrapper(routeInfo.VirtualAddress, routeInfo.ServiceIP, routeInfo.ServicePort);

                var result = await httpClient.SendAsync(msg, routeInfo.Timeout * 1000);

                httpContext.Response.Status = result.StatusCode;

                foreach (var kv in result.Headers)
                {
                    httpContext.Response.Headers.TryAdd(kv.Key, kv.Value.FirstOrDefault());
                }

                if (result.IsSuccessStatusCode)
                {
                    httpContext.Response.Write(await result.Content.ReadAsStringAsync());
                }
            }
            catch (SocketException se)
            {
                httpContext.Response.Status = System.Net.HttpStatusCode.BadRequest;
                httpContext.Response.Write(SerializeHelper.Serialize(se));
            }
            catch (TimeoutException te)
            {
                httpContext.Response.Status = System.Net.HttpStatusCode.RequestTimeout;
                httpContext.Response.Write(SerializeHelper.Serialize(te));
            }
            catch (Exception e)
            {
                httpContext.Response.Status = System.Net.HttpStatusCode.InternalServerError;
                httpContext.Response.Write(SerializeHelper.Serialize(e));
            }
            httpContext.Response.End();

            stopwatch.Stop();

            CallLog.Log(routeInfo.ServiceName, url, httpContext.Request.Headers.ToList(), Encoding.UTF8.GetString(httpContext.Response.Body), stopwatch.ElapsedMilliseconds);
        }
    }
}
