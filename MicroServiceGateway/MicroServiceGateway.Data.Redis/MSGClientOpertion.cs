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
using System.Collections.Generic;
using System.Linq;

namespace MicroServiceGateway.Data.Redis
{
    /// <summary>
    /// 客户端列表操作类
    /// </summary>
    public static class MSGClientOpertion
    {
        const string _prex = "msgclient_";

        const string _offlineZid = "msgclient_offline";

        static RedisClient _redisClient;

        /// <summary>
        /// 客户端列表操作类
        /// </summary>
        static MSGClientOpertion()
        {
            var mConfig = ManagerConfig.Read();

            _redisClient = new RedisClient(mConfig.RedisCnnStr);
        }

        static string GetZid()
        {
            return $"{_prex}ZSort";
        }

        static string GetKey(MicroServiceConfig microServiceConfig)
        {
            return GetKey(microServiceConfig.ServiceName, microServiceConfig.ServiceIP, microServiceConfig.ServicePort);
        }

        static string GetKey(string serviceName, string ip, int port)
        {
            return $"{_prex}{serviceName}{ip.Replace(".", "")}{port}";
        }

        /// <summary>
        /// 将客户端信息添加到redis
        /// </summary>
        /// <param name="microServiceConfig"></param>
        public static void Set(MicroServiceConfig microServiceConfig)
        {
            var key = GetKey(microServiceConfig);
            _redisClient.GetDataBase().Set(key, SerializeHelper.Serialize(microServiceConfig), 60);
            _redisClient.GetDataBase().ZAdd(GetZid(), key, DateTimeHelper.GetUnixTick());
        }

        /// <summary>
        /// 获取客户端信息
        /// </summary>
        /// <param name="virtualAddress"></param>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public static MicroServiceConfig Get(string virtualAddress, string ip, int port)
        {
            var key = GetKey(virtualAddress, ip, port);

            var json = _redisClient.GetDataBase().Get(key);

            if (json != null)
            {
                var result = SerializeHelper.Deserialize<MicroServiceConfig>(json);

                if (result != null)
                {
                    return result;
                }
            }
            _redisClient.GetDataBase().ZRemove(GetZid(), new string[] { key });
            return null;
        }

        /// <summary>
        /// 获取客户端信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static MicroServiceConfig Get(string key)
        {
            var json = _redisClient.GetDataBase().Get(key);

            if (json != null)
            {
                var result = SerializeHelper.Deserialize<MicroServiceConfig>(json);

                if (result != null)
                {
                    return result;
                }
            }
            _redisClient.GetDataBase().ZRemove(GetZid(), new string[] { key });

            return null;
        }

        /// <summary>
        /// 查询客户端列表
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<MicroServiceConfig> GetList()
        {
            var zis = _redisClient.GetDataBase().ZRange(GetZid());

            if (zis != null && zis.Any())
            {
                foreach (var zi in zis)
                {
                    var item = Get(zi.Value);

                    if (item != null)

                        yield return item;

                    else
                        _redisClient.GetDataBase().ZRemove(GetZid(), new string[] { zi.Value });
                }
            }
            yield break;
        }

        /// <summary>
        /// 查询客户端列表
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static IEnumerable<MicroServiceConfig> GetList(string pattern, int pageSize = 20)
        {
            var zis = _redisClient.GetDataBase().ZScan(GetZid(), 0, pattern, pageSize).Data;

            if (zis != null && zis.Any())
            {
                foreach (var zi in zis)
                {
                    var item = Get(zi.Value);

                    if (item != null)

                        yield return item;

                    else
                        _redisClient.GetDataBase().ZRemove(GetZid(), new string[] { zi.Value });
                }
            }
            yield break;
        }


        /// <summary>
        /// 更新在线状态
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        public static void KeepAlive(string serviceName, string ip, int port)
        {
            var key = GetKey(serviceName, ip, port);
            _redisClient.GetDataBase().Expire(key, 60);
        }

        /// <summary>
        /// 下线
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        public static void Offline(string serviceName, string ip, int port)
        {
            var key = $"{serviceName}{ip.Replace(".", "")}{port}";
            _redisClient.GetDataBase().ZAdd(_offlineZid, key, DateTimeHelper.GetUnixTick());
        }
        /// <summary>
        /// 上线
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        public static void Online(string serviceName, string ip, int port)
        {
            var key = $"{serviceName}{ip.Replace(".", "")}{port}";
            _redisClient.GetDataBase().ZRemove(_offlineZid, new string[] { key });
        }
    }
}
