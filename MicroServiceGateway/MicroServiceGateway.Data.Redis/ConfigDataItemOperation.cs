using MicroServiceGateway.Model;
using SAEA.Common;
using SAEA.RedisSocket;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MicroServiceGateway.Data.Redis
{
    public static class ConfigDataItemOperation
    {
        const string _prex = "configdataItem_";

        static RedisClient _redisClient;

        static string GetHid(string appId)
        {
            return $"{_prex}{appId}";
        }


        static ConfigDataItemOperation()
        {
            try
            {
                var mConfig = ManagerConfig.Read();

                _redisClient = new RedisClient(mConfig.RedisCnnStr);

                _redisClient.Connect();
            }
            catch (Exception ex)
            {
                LogHelper.Error("ConfigDataItemOperation 初始化失败", ex);
            }
        }

        public static void Set(ConfigDataItem configDataItem)
        {
            try
            {
                if (configDataItem == null || string.IsNullOrEmpty(configDataItem.ConfigDataID)) return;

                _redisClient.GetDataBase().HSet(GetHid(configDataItem.ConfigDataID), configDataItem.ID, SerializeHelper.Serialize(configDataItem));

            }
            catch (Exception ex)
            {
                LogHelper.Error("ConfigDataItemOperation.Set", ex);
            }
        }

        public static List<ConfigDataItem> GetList(string configDataID)
        {
            try
            {
                var dic = _redisClient.GetDataBase().HGetAll<ConfigDataItem>(GetHid(configDataID));

                return dic.Values.ToList();
            }
            catch (Exception ex)
            {
                LogHelper.Error("ConfigDataItemOperation.GetList", ex);
            }
            return null;
        }


        public static ConfigDataItem Get(string configDataID, string id)
        {
            try
            {
                return _redisClient.GetDataBase().HGet<ConfigDataItem>(GetHid(configDataID), id);
            }
            catch (Exception ex)
            {
                LogHelper.Error("ConfigDataItemOperation.Get", ex);
            }
            return null;
        }

        public static void Remove(string configDataID, string id)
        {
            try
            {
                _redisClient.GetDataBase().HDel(GetHid(configDataID), id);
            }
            catch (Exception ex)
            {
                LogHelper.Error("ConfigDataItemOperation.Remove", ex);
            }
        }

        public static void Remove(ConfigDataItem configDataItem)
        {
            Remove(configDataItem.ConfigDataID, configDataItem.ID);
        }

        public static void Remove(string configDataID)
        {
            try
            {
                _redisClient.GetDataBase().Del(GetHid(configDataID));
            }
            catch (Exception ex)
            {
                LogHelper.Error("ConfigDataItemOperation.Remove", ex);
            }
        }
    }
}
