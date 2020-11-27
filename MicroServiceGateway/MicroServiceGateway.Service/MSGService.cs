/****************************************************************************
*项目名称：MicroServiceGateway.Service
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：MicroServiceGateway.Service
*类 名 称：MSGService
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：yswenli@outlook.com
*创建时间：2020/8/21 13:37:24
*描述：
*=====================================================================
*修改时间：2020/8/21 13:37:24
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using MicroServiceGateway.Model;
using MicroServiceGateway.Service.Common;
using MicroServiceGateway.Service.Forwarding;
using SAEA.Common;
using SAEA.Http.Model;
using SAEA.MVC;
using SAEA.RPC.Provider;
using System;

namespace MicroServiceGateway.Service
{
    /// <summary>
    /// 微服务网关服务
    /// </summary>
    public static class MSGService
    {
        static MSGNodeConfig _msgnodeConfig;

        static SAEAMvcApplication _application;

        static ServiceProvider _rpcProvider;

        /// <summary>
        /// 微服务网关服务
        /// </summary>
        static MSGService()
        {
            Exception ex = null;

            try
            {
                _msgnodeConfig = MSGNodeConfig.Read();
            }
            catch (Exception e)
            {
                ex = e;
            }

            try
            {
                if (_msgnodeConfig == null)
                {
                    new MSGNodeConfig().Save();
                }
            }
            catch (Exception e)
            {
                ex = e;
            }

            if (ex != null)

                throw new Exception("Microservice gateway service read configuration MSGNodeConfig.yaml Failed, please try to start the service after accurate configuration");

            SAEAMvcApplicationConfig mvcConfig = SAEAMvcApplicationConfigBuilder.Read();

            mvcConfig.Port = _msgnodeConfig.NodePort;

            SAEAMvcApplicationConfigBuilder.Write(mvcConfig);

            _application = new SAEAMvcApplication(mvcConfig);

            _application.OnRequestDelegate += _application_OnRequestDelegate;

            _rpcProvider = new ServiceProvider(_msgnodeConfig.NodeRpcPort, 10240, 10);

            _rpcProvider.OnErr += _rpcProvider_OnErr;
        }

        private static void _application_OnRequestDelegate(IHttpContext context)
        {
            new RequestHandler().Invoke(context);
        }

        private static void _rpcProvider_OnErr(Exception ex)
        {
            LogHelper.Error("rpc service error", ex);
        }

        /// <summary>
        /// Start
        /// </summary>
        public static void Start()
        {
#if DEBUG
            Console.WriteLine($"{DateTimeHelper.Now} Microservice gateway service starting");
#endif
            StatisticsReporter.Start();

            _application.Start();

            _rpcProvider.Start();

#if DEBUG
            Console.WriteLine($"{DateTimeHelper.Now} Microservice gateway service started");
#endif
        }

        /// <summary>
        /// Stop
        /// </summary>
        public static void Stop()
        {
            _application.Stop();
            _rpcProvider.Stop();
        }



#if DEBUG
        /// <summary>
        /// 生成rpc客户端代码
        /// </summary>
        public static void GeneratCode()
        {
            SAEA.RPC.Generater.CodeGnerater.Generate(@"C:\Users\yswenli\Desktop\", "MicroServiceGateway.Manager", typeof(Services.NodeService));
        }
#endif
    }
}
