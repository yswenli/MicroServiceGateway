﻿/****************************************************************************
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

                throw new Exception("微服务网关服务读取配置MSGNodeConfig.yaml失败，请准确配置后再尝试启动服务");

            SAEAMvcApplicationConfig mvcConfig = SAEAMvcApplicationConfigBuilder.Read();

            mvcConfig.Port = _msgnodeConfig.NodePort;

            SAEAMvcApplicationConfigBuilder.Write(mvcConfig);

            _application = new SAEAMvcApplication(mvcConfig);

            _application.OnRequestDelegate += _application_OnRequestDelegate;

            _rpcProvider = new ServiceProvider(mvcConfig.Port + 1, mvcConfig.BufferSize, mvcConfig.Count);

            _rpcProvider.OnErr += _rpcProvider_OnErr;
        }

        private static void _application_OnRequestDelegate(IHttpContext context)
        {
            new RequestHandler(context).Invoke();
        }

        private static void _rpcProvider_OnErr(Exception ex)
        {
            LogHelper.Error("rpc service error", ex);
        }

        public static void Start()
        {
            _application.Start();

            _rpcProvider.Start();
        }

        public static void Stop()
        {
            _application.Stop();
            _rpcProvider.Stop();
        }
    }
}