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
            try
            {
                var routes = Router.Route(httpContext);

                if (routes != null)
                {
                    var routeInfo = LoadBalance.Get(routes);

                    if (routeInfo != null)

                        Forward(httpContext, routeInfo);
                }

            }
            catch (Exception ex)
            {
                LogHelper.Error("RequestHandler.Invoke", ex, httpContext);
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

            string url = $"http://{routeInfo.ServiceIP}:{routeInfo.ServicePort}";

            var request = httpContext.Request;

            var response = httpContext.Response;

            if (!string.IsNullOrEmpty(request.RelativeUrl))
            {
                var arr = request.RelativeUrl.Split("/", StringSplitOptions.RemoveEmptyEntries);

                if (arr.Length > 1)
                {
                    for (int i = 1; i < arr.Length; i++)
                    {
                        url += "/" + arr[i];
                    }
                }
            }

            var uri = new Uri(url);

            try
            {
                HttpMethod httpMethod = new HttpMethod(request.Method);

                var msg = new HttpRequestMessage()
                {
                    Method = httpMethod
                };

                foreach (var item in request.Headers)
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(item.Key) && !string.IsNullOrEmpty(item.Value) && item.Key.IndexOf("content-type", StringComparison.OrdinalIgnoreCase) == -1)
                            msg.Headers.Add(item.Key, item.Value);
                    }
                    catch (Exception ex)
                    {

                    }
                }

                msg.RequestUri = uri;

                if (request.Method == "POST")
                {
                    msg.Content = new ByteArrayContent(request.Body);
                }

                if (httpContext.Request.Headers.ContainsKey("REMOTE_ADDR"))
                {
                    if (httpContext.Request.Headers.ContainsKey("HTTP_X_FORWARDED_FOR"))
                    {
                        msg.Headers.Add("HTTP_X_FORWARDED_FOR", httpContext.Request.Headers["HTTP_X_FORWARDED_FOR"] + "," + httpContext.Request.Headers["REMOTE_ADDR"]);
                    }
                    else
                    {
                        msg.Headers.Add("HTTP_X_FORWARDED_FOR", httpContext.Request.Headers["REMOTE_ADDR"]);
                    }
                }

                var httpClient = HttpClientWrapper.GetWrapper(routeInfo.VirtualAddress, routeInfo.ServiceIP, routeInfo.ServicePort);

                var requestResult = await httpClient.SendAsync(msg, routeInfo.Timeout * 1000);

                var body = await requestResult.Content.ReadAsStringAsync();

                response.Status = requestResult.StatusCode;

                response.Write(body);
            }
            catch (SocketException se)
            {
                response.ContentType = "text/plain;charset=utf-8";
                response.Status = System.Net.HttpStatusCode.InternalServerError;
                response.Write(se.Message);
            }
            catch (TimeoutException te)
            {
                response.ContentType = "text/plain;charset=utf-8";
                response.Status = System.Net.HttpStatusCode.RequestTimeout;
                response.Write(te.Message);
            }
            catch (HttpRequestException hre)
            {
                response.ContentType = "text/plain;charset=utf-8";
                response.Status = System.Net.HttpStatusCode.NotFound;
                response.Write(hre.Message);
            }
            catch (Exception e)
            {
                response.ContentType = "text/plain;charset=utf-8";
                response.Status = System.Net.HttpStatusCode.InternalServerError;
                response.Write(e.Message);
            }

            response.End();

            stopwatch.Stop();

            CallLog.Log(routeInfo.ServiceName, uri, request.Headers.ToList(), response.Body != null ? Encoding.UTF8.GetString(response.Body) : "", stopwatch.ElapsedMilliseconds);
        }
    }
}
