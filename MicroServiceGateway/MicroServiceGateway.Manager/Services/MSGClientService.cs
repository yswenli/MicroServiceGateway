/****************************************************************************
*项目名称：MicroServiceGateway.Manager.Services
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：MicroServiceGateway.Manager.Services
*类 名 称：MSGClientService
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：yswenli@outlook.com
*创建时间：2020/8/20 14:58:31
*描述：
*=====================================================================
*修改时间：2020/8/20 14:58:31
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using MicroServiceGateway.Common;
using MicroServiceGateway.Data.Redis;
using MicroServiceGateway.Model;
using SAEA.RPC;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroServiceGateway.Manager.Services
{
    /// <summary>
    /// 微服务客户端
    /// </summary>
    [RPCService]
    public class MSGClientService
    {
        /// <summary>
        /// 客户端注册服务
        /// </summary>
        /// <param name="microServiceConfig"></param>
        /// <returns></returns>
        public bool Regist(MicroServiceConfig microServiceConfig)
        {
            try
            {
                Console.WriteLine($"microServiceConfig:{SAEA.Common.SerializeHelper.Serialize(microServiceConfig)}");
                MSGClientOpertion.Set(microServiceConfig);
            }
            catch(Exception ex)
            {
                Logger.Error("Microservice client failed to register service", ex);
            }
            return true;
        }

        /// <summary>
        /// 客户端资源上报
        /// </summary>
        /// <param name="performaceModel"></param>
        /// <returns></returns>
        public bool Report(PerformaceModel performaceModel)
        {
            Console.WriteLine($"performaceModel:{SAEA.Common.SerializeHelper.Serialize(performaceModel)}");
            return true;
        }
    }
}
