/****************************************************************************
*项目名称：MicroServiceGateway.Client
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：MicroServiceGateway.Client
*类 名 称：MSGMiddleware
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：yswenli@outlook.com
*创建时间：2020/8/20 13:54:01
*描述：
*=====================================================================
*修改时间：2020/8/20 13:54:01
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using MicroServiceGateway.Model;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MicroServiceGateway.Client
{
    /// <summary>
    /// 微服务客户中间件
    /// </summary>
    public class MSGMiddleware
    {
        private readonly RequestDelegate _next;

        MicroServiceConfig _microServiceConfig = null;

        public MSGMiddleware(RequestDelegate next)
        {
            _next = next;

            try
            {
                if (_microServiceConfig == null)
                {
                    _microServiceConfig = MicroServiceConfig.Read();


                }                
            }
            catch (Exception ex)
            {
                throw new Exception("初始化MicroServiceConfig配置失败，请检查MicroServiceConfig配置文件及其内容是否正确", ex);
            }
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await _next(context);
        }
    }
}
