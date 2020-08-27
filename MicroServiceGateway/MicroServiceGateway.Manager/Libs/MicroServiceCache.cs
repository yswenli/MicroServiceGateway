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
using MicroServiceGateway.Model;
using SAEA.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MicroServiceGateway.Manager.Libs
{
    /// <summary>
    /// 微服务缓存
    /// </summary>
    public static class MicroServiceCache
    {
        static MemoryCacheHelper<MicroServiceConfig> _memoryCache;

        /// <summary>
        /// 微服务缓存
        /// </summary>
        static MicroServiceCache()
        {
            _memoryCache = new MemoryCacheHelper<MicroServiceConfig>();
        }

        static string GetKey(MicroServiceConfig microService)
        {
            return GetKey(microService.VirtualAddress, microService.ServiceIP, microService.ServicePort);
        }

        static string GetKey(string virtualAddress, string ip, int port)
        {
            return $"{virtualAddress}{ip}{port}";
        }

        public static void Set(MicroServiceConfig microService)
        {
            _memoryCache.Set(GetKey(microService), microService, TimeSpan.FromSeconds(60));
        }

        public static void KeepAlive(string virtualAddress, string ip, int port)
        {
            _memoryCache.Active(GetKey(virtualAddress, ip, port), TimeSpan.FromSeconds(60));
        }
    }
}
