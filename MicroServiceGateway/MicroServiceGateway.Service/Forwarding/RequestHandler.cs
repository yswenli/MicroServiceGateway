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
using SAEA.Http.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroServiceGateway.Service.Forwarding
{
    /// <summary>
    /// webhost请求处理
    /// </summary>
    public class RequestHandler
    {
        IHttpContext _httpContext;

        /// <summary>
        /// webhost请求处理
        /// </summary>
        /// <param name="httpContext"></param>
        public RequestHandler(IHttpContext httpContext)
        {
            _httpContext = httpContext;
        }

        /// <summary>
        /// 处理
        /// </summary>
        public void Invoke()
        {

        }
    }
}
