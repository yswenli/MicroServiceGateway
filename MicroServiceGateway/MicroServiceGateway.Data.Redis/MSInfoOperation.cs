using MicroServiceGateway.Model;
using SAEA.Common;
using SAEA.RedisSocket;
using System;
using System.Collections.Generic;
using System.Linq;

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
            try
            {
                var mConfig = ManagerConfig.Read();

                _redisClient = new RedisClient(mConfig.RedisCnnStr);

                _redisClient.Connect();
            }
            catch (Exception ex)
            {
                LogHelper.Error("MSInfoOperation 初始化失败", ex);
            }
        }

        /// <summary>
        /// Set
        /// </summary>
        /// <param name="microServiceConfig"></param>
        public static void Set(MicroServiceConfig microServiceConfig)
        {
            try
            {
                _redisClient.GetDataBase().HSet(GetKey(microServiceConfig.VirtualAddress), microServiceConfig.ServiceIP + microServiceConfig.ServicePort, SerializeHelper.Serialize(microServiceConfig));
                _redisClient.GetDataBase().SAdd("msinfo_virtualAddress", microServiceConfig.VirtualAddress);
            }
            catch (Exception ex)
            {
                LogHelper.Error("MSInfoOperation Set失败", ex);
            }
        }

        /// <summary>
        /// SetExpired
        /// </summary>
        /// <param name="virtualAddress"></param>
        /// <param name="serviceIP"></param>
        /// <param name="servicePort"></param>
        public static void SetOnline(string virtualAddress, string serviceIP, int servicePort)
        {
            try
            {
                _redisClient.GetDataBase().Set($"{virtualAddress}{serviceIP}{servicePort}", DateTimeHelper.Now.Ticks.ToString());
                _redisClient.GetDataBase().Expire($"{virtualAddress}{serviceIP}{servicePort}", 60);
            }
            catch (Exception ex)
            {
                LogHelper.Error("MSInfoOperation SetOnline 失败", ex);
            }
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
            try
            {
                var json = _redisClient.GetDataBase().HGet(GetKey(virtualAddress), serviceIP + servicePort);
                if (!string.IsNullOrEmpty(json) && json != "One or more errors occurred. (A task was canceled.)")
                {
                    return SerializeHelper.Deserialize<MicroServiceConfig>(json);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("MSInfoOperation Get 失败", ex);
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
            try
            {
                _redisClient.GetDataBase().Del($"{virtualAddress}{serviceIP}{servicePort}");
                return _redisClient.GetDataBase().HDel(GetKey(virtualAddress), serviceIP + servicePort) > 0;
            }
            catch (Exception ex)
            {
                LogHelper.Error("MSInfoOperation Del 失败", ex);
            }
            return false;
        }

        /// <summary>
        /// 获取虚拟地扯
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<string> GetVirtualAddress()
        {
            try
            {
                return _redisClient.GetDataBase().SMemebers("msinfo_virtualAddress");
            }
            catch (Exception ex)
            {
                LogHelper.Error("MSInfoOperation GetVirtualAddress 失败", ex);
            }
            return null;
        }

    }
}
