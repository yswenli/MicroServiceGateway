using MicroServiceGateway.Model;
using SAEA.Common;
using SAEA.RedisSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MicroServiceGateway.Data.Redis
{
    /// <summary>
    /// 微服务记录操作类
    /// </summary>
    public static class MSInfoOperation
    {
        const string _prex = "msinfo_";

        static RedisClient _redisClient;

        static string GetKey(string virtualAddress)
        {
            return $"{_prex}{virtualAddress}";
        }

        /// <summary>
        /// 微服务记录操作类
        /// </summary>
        static MSInfoOperation()
        {
            var mConfig = ManagerConfig.Read();

            _redisClient = new RedisClient(mConfig.RedisCnnStr);

            _redisClient.Connect();
        }

        /// <summary>
        /// Set
        /// </summary>
        /// <param name="microServiceConfig"></param>
        public static void Set(MicroServiceConfig microServiceConfig)
        {
            _redisClient.GetDataBase().HSet(GetKey(microServiceConfig.VirtualAddress), microServiceConfig.ServiceIP + microServiceConfig.ServicePort, SerializeHelper.Serialize(microServiceConfig));
            _redisClient.GetDataBase().SAdd("msinfo_virtualAddress", microServiceConfig.VirtualAddress);
        }

        /// <summary>
        /// SetExpired
        /// </summary>
        /// <param name="virtualAddress"></param>
        /// <param name="serviceIP"></param>
        /// <param name="servicePort"></param>
        public static void SetOnline(string virtualAddress, string serviceIP, int servicePort)
        {
            _redisClient.GetDataBase().Set($"{virtualAddress}{serviceIP}{servicePort}", DateTimeHelper.Now.Ticks.ToString());
            _redisClient.GetDataBase().Expire($"{virtualAddress}{serviceIP}{servicePort}", 60);
        }
        /// <summary>
        /// GetOnline
        /// </summary>
        /// <param name="virtualAddress"></param>
        /// <param name="serviceIP"></param>
        /// <param name="servicePort"></param>
        /// <returns></returns>
        public static bool GetOnline(string virtualAddress, string serviceIP, int servicePort)
        {
            try
            {
                return _redisClient.GetDataBase().Exists($"{virtualAddress}{serviceIP}{servicePort}");
            }
            catch { }
            return false;
        }

        /// <summary>
        /// Get
        /// </summary>
        /// <param name="virtualAddress"></param>
        /// <param name="serviceIP"></param>
        /// <param name="servicePort"></param>
        /// <returns></returns>
        public static MicroServiceConfig Get(string virtualAddress, string serviceIP, int servicePort)
        {
            var json = _redisClient.GetDataBase().HGet(GetKey(virtualAddress), serviceIP + servicePort);
            if (!string.IsNullOrEmpty(json) && json!= "One or more errors occurred. (A task was canceled.)")
            {
                return SerializeHelper.Deserialize<MicroServiceConfig>(json);
            }
            return null;
        }

        /// <summary>
        /// GetList
        /// </summary>
        /// <param name="virtualAddress"></param>
        /// <returns></returns>
        public static IEnumerable<MicroServiceConfig> GetList(string virtualAddress)
        {
            var dic = _redisClient.GetDataBase().HGetAll(GetKey(virtualAddress));
            if (dic != null && dic.Any())
            {
                foreach (var item in dic)
                {
                    if (!string.IsNullOrEmpty(item.Value))
                    {
                        yield return SerializeHelper.Deserialize<MicroServiceConfig>(item.Value);
                    }
                }
            }
            yield break;
        }

        /// <summary>
        /// del
        /// </summary>
        /// <param name="virtualAddress"></param>
        /// <param name="serviceIP"></param>
        /// <param name="servicePort"></param>
        public static bool Del(string virtualAddress, string serviceIP, int servicePort)
        {
            _redisClient.GetDataBase().Del($"{virtualAddress}{serviceIP}{servicePort}");
            return _redisClient.GetDataBase().HDel(GetKey(virtualAddress), serviceIP + servicePort) > 0;
        }

        /// <summary>
        /// 获取虚拟地扯
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<string> GetVirtualAddress()
        {
            return _redisClient.GetDataBase().SMemebers("msinfo_virtualAddress");
        }

    }
}
