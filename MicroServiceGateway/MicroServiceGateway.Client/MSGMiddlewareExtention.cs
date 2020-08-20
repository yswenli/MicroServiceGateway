/****************************************************************************
*项目名称：MicroServiceGateway.Client
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：MicroServiceGateway.Client
*类 名 称：MSGMiddlewareExtention
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：yswenli@outlook.com
*创建时间：2020/8/20 15:56:17
*描述：
*=====================================================================
*修改时间：2020/8/20 15:56:17
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using Microsoft.AspNetCore.Builder;

namespace MicroServiceGateway.Client
{
    public static class MSGMiddlewareExtention
    {
        /// <summary>
        /// 使用微服务中间件
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseMSGMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MSGMiddleware>();
        }
    }
}
