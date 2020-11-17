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
using MicroServiceGateway.Manager.ServiceDiscovery;
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
        static SAEAMvcApplication _application;

        static ServiceProvider _rpcProvider;

        /// <summary>
        /// 管理端服务
        /// </summary>
        static ManagerService()
        {
            SAEAMvcApplicationConfig mvcConfig = SAEAMvcApplicationConfigBuilder.Read();

            _application = new SAEAMvcApplication(mvcConfig);

            _rpcProvider = new ServiceProvider(mvcConfig.Port + 1, mvcConfig.BufferSize, mvcConfig.Count);

            _rpcProvider.OnErr += _rpcProvider_OnErr;

            MicroServiceCache.OnChanged += MicroServiceCache_OnChanged;
        }

        private async static void MicroServiceCache_OnChanged(bool isAdd, Model.MicroServiceConfig microServiceConfig)
        {
            //todo
        }

        private static void _rpcProvider_OnErr(Exception ex)
        {
            LogHelper.Error("rpc service error", ex);
        }

        /// <summary>
        /// 启动服务
        /// </summary>
        public static void Start()
        {
            _application.Start();
            _rpcProvider.Start();
            MSGNodeRPCServiceDic.Start();
        }

        /// <summary>
        /// 关闭服务
        /// </summary>
        public static void Stop()
        {
            MSGNodeRPCServiceDic.Stop();
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
