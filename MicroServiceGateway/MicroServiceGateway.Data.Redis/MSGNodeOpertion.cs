/****************************************************************************
*项目名称：MicroServiceGateway.Data.Redis
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：MicroServiceGateway.Data.Redis
*类 名 称：MSGNodeOpertion
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：yswenli@outlook.com
*创建时间：2020/8/21 13:14:58
*描述：
*=====================================================================
*修改时间：2020/8/21 13:14:58
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
    /// redis网关节点列表操作类
    /// </summary>
    public static class MSGNodeOpertion
    {
        const string _prex = "msgnode_";

        static RedisClient _redisClient;

        /// <summary>
        /// redis网关节点列表操作类
        /// </summary>
        static MSGNodeOpertion()
        {
            var mConfig = ManagerConfig.Read();

            _redisClient = new RedisClient(mConfig.RedisCnnStr);

            _redisClient.Connect();
        }

        static string GetZid()
        {
            return $"{_prex}ZSort";
        }

        static string GetKey(MSGNodeInfo msgNode)
        {
            return GetKey(msgNode.NodeName);
        }

        static string GetKey(string nodeName)
        {
            return $"{_prex}{nodeName}";
        }

        /// <summary>
        /// 将网关节点信息添加到集合
        /// </summary>
        /// <param name="msgNode"></param>
        public static void Set(MSGNodeInfo msgNode)
        {
            var key = GetKey(msgNode.NodeName);
            _redisClient.GetDataBase().Set(key, SerializeHelper.Serialize(msgNode));
            _redisClient.GetDataBase().ZAdd(GetZid(), key, DateTimeHelper.GetUnixTick());
        }

        /// <summary>
        /// 将网关节点信息添加到集合
        /// </summary>
        /// <param name="msgNodes"></param>
        public static void Set(List<MSGNodeInfo> msgNodes)
        {
            if(msgNodes!=null && msgNodes.Any())
            {
                foreach (var msgNode in msgNodes)
                {
                    Set(msgNode);
                }
            }
        }

        /// <summary>
        /// 获取网关节点信息
        /// </summary>
        /// <param name="nodeName"></param>
        /// <returns></returns>
        public static MSGNodeInfo Get(string nodeName)
        {
            var key = nodeName;

            if (nodeName.IndexOf(_prex) == -1)
            {
                key = GetKey(nodeName);
            }

            var json = _redisClient.GetDataBase().Get(key);

            if (json != null)
            {
                var result = SerializeHelper.Deserialize<MSGNodeInfo>(json);

                if (result != null)
                {
                    return result;
                }
            }
            _redisClient.GetDataBase().ZRemove(GetZid(), new string[] { key });
            return null;
        }

        /// <summary>
        /// 是否已存在网关节点信息
        /// </summary>
        /// <param name="nodeName"></param>
        /// <returns></returns>
        public static bool Exists(string nodeName)
        {
            var key = nodeName;

            if (nodeName.IndexOf(_prex) == -1)
            {
                key = GetKey(nodeName);
            }
            return _redisClient.GetDataBase().Exists(key);
        }

        /// <summary>
        /// 查询网关节点列表
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<MSGNodeInfo> GetList()
        {
            var zis = _redisClient.GetDataBase().ZRange(GetZid());

            if (zis != null && zis.Any())
            {
                foreach (var zi in zis)
                {
                    var item = Get(zi.Value.Trim());

                    if (item != null)

                        yield return item;

                    else
                        _redisClient.GetDataBase().ZRemove(GetZid(), new string[] { zi.Value });
                }
            }
            yield break;
        }
        /// <summary>
        /// 移除网关节点
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        public static void Del(string nodeName)
        {
            var key = nodeName;

            if (nodeName.IndexOf(_prex) == -1)
            {
                key = GetKey(nodeName);
            }
            _redisClient.GetDataBase().Del(key);
            _redisClient.GetDataBase().ZRemove(GetZid(), new string[] { key });
        }
    }
}
