/****************************************************************************
*项目名称：MicroServiceGateway.Data.Redis
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：MicroServiceGateway.Data.Redis
*类 名 称：MSGEnvOperation
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：yswenli@outlook.com
*创建时间：2020/8/22 17:57:37
*描述：
*=====================================================================
*修改时间：2020/8/22 17:57:37
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using MicroServiceGateway.Model;
using SAEA.Common;
using SAEA.RedisSocket;
using System.Collections.Generic;

namespace MicroServiceGateway.Data.Redis
{
    /// <summary>
    /// redis环境管理操作类
    /// </summary>
    public class MSGEnvOperation
    {
        const string _prex = "msgenv_";

        static RedisClient _redisClient;

        /// <summary>
        /// redis环境管理操作类
        /// </summary>
        static MSGEnvOperation()
        {
            var mConfig = ManagerConfig.Read();

            _redisClient = new RedisClient(mConfig.RedisCnnStr);
        }

        /// <summary>
        /// 添加环境
        /// </summary>
        /// <param name="env"></param>
        public static void Set(string env)
        {
            _redisClient.GetDataBase().ZAdd(_prex, env, DateTimeHelper.GetUnixTick());
        }
        /// <summary>
        /// 移除环境
        /// </summary>
        /// <param name="env"></param>
        public static void Remove(string env)
        {
            _redisClient.GetDataBase().ZRemove(_prex, new string[] { env });
        }

        /// <summary>
        /// 添加环境
        /// </summary>
        /// <param name="cluster"></param>
        public static List<string> GetList()
        {
            return _redisClient.GetDataBase().ZrangeByLex(_prex, double.MinValue, double.MaxValue);
        }
    }
}
