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
        static MemoryCacheHelper<MicroServiceConfig> _memoryCache;

        static ConcurrentDictionary<string, List<string>> _dic;

        /// <summary>
        /// 微服务端发生变化时事件
        /// </summary>
        public static event OnChangedHandler OnChanged;

        /// <summary>
        /// 微服务缓存
        /// </summary>
        static MicroServiceCache()
        {
            _memoryCache = new MemoryCacheHelper<MicroServiceConfig>();

            _memoryCache.OnChanged += _memoryCache_OnChanged;

            _dic = new ConcurrentDictionary<string, List<string>>();
        }

        private static void _memoryCache_OnChanged(bool arg1, MicroServiceConfig arg2)
        {
            if (!arg1)
            {
                if (arg2 == null)
                {
                    _dic.TryRemove(arg2.VirtualAddress, out List<string> strs);
                }
                else
                {
                    if (_dic.ContainsKey(arg2.VirtualAddress))
                    {
                        _dic[arg2.VirtualAddress].Remove(arg2.ServiceIP + arg2.ServicePort);
                    }
                }
            }
            OnChanged?.Invoke(arg1, arg2);
        }
        static string GetKey(MicroServiceConfig microService)
        {
            return GetKey(microService.VirtualAddress, microService.ServiceIP, microService.ServicePort);
        }

        static string GetKey(string virtualAddress, string serviceIP, int servicePort)
        {
            return $"{virtualAddress}{serviceIP}{servicePort}";
        }
        /// <summary>
        /// 更新微服务信息
        /// </summary>
        /// <param name="microService"></param>
        /// <returns></returns>
        public static bool Set(MicroServiceConfig microService)
        {
            var ipport = $"{microService.ServiceIP}{microService.ServicePort}";

            _dic.AddOrUpdate(microService.VirtualAddress, new List<string>() { ipport }, (k, v) =>
            {
                if (!v.Exists(b => b == ipport))
                {
                    v.Add(ipport);
                }
                return v;
            });
            _memoryCache.Set(GetKey(microService), microService, TimeSpan.FromSeconds(60));
            return true;
        }

        /// <summary>
        /// 调整缓存过期时间
        /// </summary>
        /// <param name="virtualAddress"></param>
        /// <param name="serviceIP"></param>
        /// <param name="servicePort"></param>
        public static void KeepAlive(string virtualAddress, string serviceIP, int servicePort)
        {
            _memoryCache.Active(GetKey(virtualAddress, serviceIP, servicePort), TimeSpan.FromSeconds(60));
        }

        /// <summary>
        /// 获取注册的微服务列表
        /// </summary>
        /// <param name="virtualAddress"></param>
        /// <returns></returns>
        public static List<MicroServiceConfig> GetList(string virtualAddress)
        {
            List<MicroServiceConfig> result = null;

            if (_dic.TryGetValue(virtualAddress, out List<string> ipports))
            {
                if (ipports != null && ipports.Any())
                {
                    result = new List<MicroServiceConfig>();

                    foreach (var ipport in ipports)
                    {
                        var msi = _memoryCache.Get($"{virtualAddress}{ipport}");
                        if (msi != null)
                        {
                            result.Add(msi);
                        }
                        else
                        {
                            _dic[virtualAddress].Remove(ipport);
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 获取VirualAddress
        /// </summary>
        /// <returns></returns>
        public static List<string> GetVirualAddress()
        {
            if (!_dic.IsEmpty)
            {
                return _dic.Keys.ToList();
            }
            else
            {
                return null;
            }
        }
    }
}
