using MicroServiceGateway.Model;
using SAEA.Common;
using SAEA.RedisSocket;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MicroServiceGateway.Data.Redis
{
    public static class AppDataOperation
    {
        const string _prex = "appdata";

        static RedisClient _redisClient;

        static string GetHid()
        {
            return $"{_prex}";
        }

        static AppDataOperation()
        {
            try
            {
                var mConfig = ManagerConfig.Read();

                _redisClient = new RedisClient(mConfig.RedisCnnStr);

                _redisClient.Connect();
            }
            catch (Exception ex)
            {
                LogHelper.Error("AppDataOperation 初始化失败", ex);
            }
        }

        public static void Set(AppData appData)
        {
            try
            {
                if (appData == null) return;

                if (string.IsNullOrEmpty(appData.ID)) appData.ID = IdWorker.GetId().ToString();

                _redisClient.GetDataBase().HSet(GetHid(), appData.ID, SerializeHelper.Serialize(appData));

            }
            catch (Exception ex)
            {
                LogHelper.Error("AppDataOperation.Set", ex);
            }
        }

        public static List<AppData> GetList()
        {
            try
            {
                var dic = _redisClient.GetDataBase().HGetAll<AppData>(GetHid());

                return dic.Values.ToList();
            }
            catch (Exception ex)
            {
                LogHelper.Error("AppDataOperation.GetList", ex);
            }
            return null;
        }


        public static AppData Get(string id)
        {
            try
            {
                return _redisClient.GetDataBase().HGet<AppData>(GetHid(), id);
            }
            catch (Exception ex)
            {
                LogHelper.Error("AppDataOperation.Get", ex);
            }
            return null;
        }


        public static bool Remove(string id)
        {
            try
            {
                return _redisClient.GetDataBase().HDel(GetHid(), id) > 0 ? true : false;
            }
            catch (Exception ex)
            {
                LogHelper.Error("AppDataOperation.Remove", ex);
            }
            return false;
        }


    }
}
