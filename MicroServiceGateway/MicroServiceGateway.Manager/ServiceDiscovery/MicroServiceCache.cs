/****************************************************************************
*项目名称：MicroServiceGateway.Manager.Libs
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：MicroServiceGateway.Manager.Libs
*类 名 称：MicroServiceCache
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：yswenli@outlook.com
*创建时间：2020/8/25 17:41:53
*描述：
*=====================================================================
*修改时间：2020/8/25 17:41:53
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using MicroServiceGateway.Data.Redis;
using MicroServiceGateway.Model;
using SAEA.Common;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MicroServiceGateway.Manager.ServiceDiscovery
{
    public delegate void OnChangedHandler(bool isAdd, MicroServiceConfig microServiceConfig);

    /// <summary>
    /// 微服务缓存
    /// </summary>
    public static class MicroServiceCache
    {
        /// <summary>
        /// 更新微服务信息
        /// </summary>
        /// <param name="microService"></param>
        /// <returns></returns>
        public static void Set(MicroServiceConfig microService)
        {
            MSInfoOperation.Set(microService);
        }

        /// <summary>
        /// 调整缓存过期时间
        /// </summary>
        /// <param name="virtualAddress"></param>
        /// <param name="serviceIP"></param>
        /// <param name="servicePort"></param>
        public static void SetOnline(string virtualAddress, string serviceIP, int servicePort)
        {
            MSInfoOperation.SetOnline(virtualAddress, serviceIP, servicePort);
        }
        /// <summary>
        /// 调整缓存过期时间
        /// </summary>
        /// <param name="virtualAddress"></param>
        /// <param name="serviceIP"></param>
        /// <param name="servicePort"></param>
        /// <returns></returns>
        public static bool GetOnline(string virtualAddress, string serviceIP, int servicePort)
        {
            return MSInfoOperation.GetOnline(virtualAddress, serviceIP, servicePort);
        }

        /// <summary>
        /// Del
        /// </summary>
        /// <param name="virtualAddress"></param>
        /// <param name="serviceIP"></param>
        /// <param name="servicePort"></param>
        public static bool Del(string virtualAddress, string serviceIP, int servicePort)
        {
            return MSInfoOperation.Del(virtualAddress, serviceIP, servicePort);
        }

        /// <summary>
        /// 获取注册的微服务列表
        /// </summary>
        /// <param name="virtualAddress"></param>
        /// <returns></returns>
        public static IEnumerable<MicroServiceConfig> GetList(string virtualAddress)
        {
            return MSInfoOperation.GetList(virtualAddress);
        }

        /// <summary>
        /// 获取VirualAddress
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<string> GetVirualAddress()
        {
            return MSInfoOperation.GetVirtualAddress();
        }
    }
}
