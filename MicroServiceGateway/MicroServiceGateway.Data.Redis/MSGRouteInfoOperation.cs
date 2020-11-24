/****************************************************************************
*项目名称：MicroServiceGateway.Manager.Services
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：MicroServiceGateway.Data.Redis
*类 名 称：MSGRouteInfoOperation
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：yswenli@outlook.com
*创建时间：2020/8/23 14:58:31
*描述：
*=====================================================================
*修改时间：2020/8/23 14:58:31
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using MicroServiceGateway.Model;
using SAEA.Common;
using SAEA.RedisSocket;
using System;
using System.Collections.Generic;

namespace MicroServiceGateway.Data.Redis
{
    /// <summary>
    /// 路由信息
    /// </summary>
    public static class MSGRouteInfoOperation
    {
        const string _prex = "msgrouteinfo";

        static RedisClient _redisClient;

        /// <summary>
        /// redis环境管理操作类
        /// </summary>
        static MSGRouteInfoOperation()
        {
            try
            {
                var mConfig = ManagerConfig.Read();

                _redisClient = new RedisClient(mConfig.RedisCnnStr);

                _redisClient.Connect();
            }
            catch(Exception ex)
            {
                LogHelper.Error("MSGRouteInfoOperation.Init", ex);
            }
        }
        /// <summary>
        /// 设置路由
        /// </summary>
        /// <param name="routeInfos"></param>
        public static void Write(List<RouteInfo> routeInfos)
        {
            try
            {
                var json = SerializeHelper.Serialize(routeInfos);
                _redisClient.GetDataBase().Set(_prex, json);
            }
            catch(Exception ex)
            {
                LogHelper.Error("MSGRouteInfoOperation.Write", ex);
            }
        }
        /// <summary>
        /// 获取路由列表
        /// </summary>
        /// <returns></returns>
        public static List<RouteInfo> Read()
        {
            try
            {
                var json = _redisClient.GetDataBase().Get(_prex);

                if (!string.IsNullOrEmpty(json))
                {
                    return SerializeHelper.Deserialize<List<RouteInfo>>(json);
                }
            }
            catch(Exception ex)
            {
                LogHelper.Error("MSGRouteInfoOperation.Read", ex);
            }
            return new List<RouteInfo>();
        }
    }
}
