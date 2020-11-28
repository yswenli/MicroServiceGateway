/****************************************************************************
*项目名称：MicroServiceGateway.Manager
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：MicroServiceGateway.Manager
*类 名 称：ManagerService
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：yswenli@outlook.com
*创建时间：2020/8/20 14:43:34
*描述：
*=====================================================================
*修改时间：2020/8/20 14:43:34
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using MicroServiceGateway.Manager.Libs;
using SAEA.Common;
using SAEA.MVC;
using SAEA.RPC.Provider;
using System;

namespace MicroServiceGateway.Manager
{
    /// <summary>
    /// 管理端服务
    /// </summary>
    public static class ManagerService
    {
        static SAEAMvcApplicationConfig _mvcConfig;

        static SAEAMvcApplication _application;

        static ServiceProvider _rpcProvider;

        /// <summary>
        /// 管理端服务
        /// </summary>
        static ManagerService()
        {
            _mvcConfig = SAEAMvcApplicationConfigBuilder.Read();

            _application = new SAEAMvcApplication(_mvcConfig);

            _application.OnException += _application_OnException;

            _rpcProvider = new ServiceProvider(_mvcConfig.Port + 1, _mvcConfig.BufferSize, _mvcConfig.Count);

            _rpcProvider.OnErr += _rpcProvider_OnErr;
        }

        private static SAEA.Http.Model.IHttpResult _application_OnException(SAEA.Http.Model.IHttpContext httpContext, Exception ex)
        {
#if DEBUG
            ConsoleHelper.WriteLine("ManagerService error:" + SerializeHelper.Serialize(ex));
#endif
            LogHelper.Error("ManagerService error", ex);

            return new ContentResult("ManagerService error:" + SerializeHelper.Serialize(ex));
        }

        private static void _rpcProvider_OnErr(Exception ex)
        {
#if DEBUG
            ConsoleHelper.WriteLine("ManagerService error:" + SerializeHelper.Serialize(ex));
#endif
            LogHelper.Error("ManagerService error", ex);
        }

        /// <summary>
        /// 启动服务
        /// </summary>
        public static void Start()
        {
#if DEBUG
            ConsoleHelper.WriteLine("MicroServiceGateway.Manager 正在启动...");
#endif

            _application.Start();
            _rpcProvider.Start();
            MSGNodeRPCServiceCache.Start();

#if DEBUG
            ConsoleHelper.WriteLine("MicroServiceGateway.Manager 已启动");
            ConsoleHelper.WriteLine($"打开 http://127.0.0.1:{_mvcConfig.Port}/ 查看相关信息");
#endif
        }

        /// <summary>
        /// 关闭服务
        /// </summary>
        public static void Stop()
        {
            MSGNodeRPCServiceCache.Stop();
            _application.Stop();
            _rpcProvider.Stop();
        }

#if DEBUG
        /// <summary>
        /// 生成rpc客户端代码
        /// </summary>
        public static void GeneratCode()
        {
            SAEA.RPC.Generater.CodeGnerater.Generate(@"C:\Users\yswenli\Desktop", "MicroServiceGateway.Client", typeof(Services.MSGClientService));
        }
#endif
    }
}
