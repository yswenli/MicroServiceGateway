/****************************************************************************
*项目名称：MicroServiceGateway.Data.Redis
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：MicroServiceGateway.Data.Redis
*类 名 称：MSGClientOpertion
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：yswenli@outlook.com
*创建时间：2020/8/20 17:40:37
*描述：
*=====================================================================
*修改时间：2020/8/20 17:40:37
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using MicroServiceGateway.Model;
using SAEA.Common;
using SAEA.RedisSocket;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroServiceGateway.Data.Redis
{
    public static class MSGClientOpertion
    {
        const string _prex = "msgclient_";

        static RedisClient _redisClient;

        static MSGClientOpertion()
        {
            var mConfig = ManagerConfig.Read();

            _redisClient = new RedisClient(mConfig.RedisCnnStr);
        }

        static string GetHid()
        {
            return $"{_prex}list";
        }

        static string GetKey(MicroServiceConfig microServiceConfig)
        {
            return GetKey(microServiceConfig.ServiceName, microServiceConfig.IP, microServiceConfig.Port);
        }

        static string GetKey(string serviceName, string ip, int port)
        {
            return $"{serviceName}{ip.Replace(".", "")}{port}";
        }

        /// <summary>
        /// 将客户端信息添加到redis
        /// </summary>
        /// <param name="microServiceConfig"></param>
        public static void Set(MicroServiceConfig microServiceConfig)
        {
            var key = GetKey(microServiceConfig);
            _redisClient.GetDataBase().Set(key, SerializeHelper.Serialize(microServiceConfig), 60);
            _redisClient.GetDataBase().ZAdd(GetHid(), key, DateTimeHelper.GetUnixTick());
        }
    }
}
