using MicroServiceGateway.Model;
using SAEA.Common;
using SAEA.RedisSocket;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MicroServiceGateway.Data.Redis
{
    /// <summary>
    /// api访问计数器
    /// </summary>
    public static class ApiCounterOperation
    {
        const string _prex = "apicounter_";

        static RedisClient _redisClient;


        static string GetHid()
        {
            return $"apicounter_{DateTimeHelper.Now.ToString("yyyyMMdd")}";
        }

        /// <summary>
        /// api访问计数器
        /// </summary>
        static ApiCounterOperation()
        {
            try
            {
                var mConfig = ManagerConfig.Read();

                _redisClient = new RedisClient(mConfig.RedisCnnStr);

                _redisClient.Connect();
            }
            catch (Exception ex)
            {
                LogHelper.Error("MSGNodeOperation.Init", ex);
            }
        }

        /// <summary>
        /// set api访问计数器
        /// </summary>
        /// <param name="url"></param>
        /// <param name="count"></param>
        public static void Set(string url, long count)
        {
            Set(new Apistatistical()
            {
                Url = url,
                Count = count
            });
        }

        /// <summary>
        /// set api访问计数器
        /// </summary>
        /// <param name="apistatistical"></param>
        public static void Set(Apistatistical apistatistical)
        {
            try
            {
                _redisClient.GetDataBase().HIncrementBy(GetHid(), apistatistical.Url, (int)apistatistical.Count);
            }
            catch (Exception ex)
            {
                LogHelper.Error("ApiCounterOperation.Set", ex, apistatistical);
            }
        }

        /// <summary>
        /// get api访问计数器
        /// </summary>
        /// <returns></returns>
        public static List<Apistatistical> GetApistatisticals()
        {
            var result = new List<Apistatistical>();

            try
            {
                var dic = _redisClient.GetDataBase().HGetAll(GetHid());

                if (dic != null && dic.Any())
                {
                    foreach (var item in dic)
                    {
                        result.Add(new Apistatistical()
                        {
                            Url = item.Key,
                            Count = long.Parse(item.Value)
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("ApiCounterOperation.GetApistatisticals", ex);
            }
            return result;
        }
    }
}
