using MicroServiceGateway.Model;
using SAEA.Common;
using SAEA.RedisSocket;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MicroServiceGateway.Data.Redis
{
    public static class ConfigDataOperation
    {
        const string _prex = "configdata_";

        static RedisClient _redisClient;

        static string GetHid(string appId)
        {
            return $"{_prex}{appId}";
        }

        static ConfigDataOperation()
        {
            try
            {
                var mConfig = ManagerConfig.Read();

                _redisClient = new RedisClient(mConfig.RedisCnnStr);

                _redisClient.Connect();
            }
            catch (Exception ex)
            {
                LogHelper.Error("ConfigDataOperation 初始化失败", ex);
            }
        }

        public static void Set(ConfigData configData)
        {
            try
            {
                if (configData == null || string.IsNullOrEmpty(configData.AppID)) return;

                _redisClient.GetDataBase().HSet(GetHid(configData.AppID), configData.Envirment, SerializeHelper.Serialize(configData));

            }
            catch (Exception ex)
            {
                LogHelper.Error("ConfigDataOperation.Set", ex);
            }
        }

        public static List<ConfigData> GetList(string appId)
        {
            try
            {
                var dic = _redisClient.GetDataBase().HGetAll<ConfigData>(GetHid(appId));

                return dic.Values.ToList();
            }
            catch (Exception ex)
            {
                LogHelper.Error("ConfigDataOperation.GetList", ex);
            }
            return null;
        }


        public static ConfigData Get(string appId, string env)
        {
            try
            {
                return _redisClient.GetDataBase().HGet<ConfigData>(GetHid(appId), env);
            }
            catch (Exception ex)
            {
                LogHelper.Error("ConfigDataOperation.Get", ex);
            }
            return null;
        }


        public static void Remove(string appId)
        {
            try
            {
                _redisClient.GetDataBase().Del(GetHid(appId));
            }
            catch (Exception ex)
            {
                LogHelper.Error("ConfigDataOperation.Remove", ex);
            }
        }

    }
}
