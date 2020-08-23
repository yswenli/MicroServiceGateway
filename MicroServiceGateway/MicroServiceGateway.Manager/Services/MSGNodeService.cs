/****************************************************************************
*项目名称：MicroServiceGateway.Manager.Services
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：MicroServiceGateway.Manager.Services
*类 名 称：MSGNodeService
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
using SAEA.RPC;
using System;
using System.Collections.Generic;

namespace MicroServiceGateway.Manager.Services
{
    /// <summary>
    /// 微服务节点管理服务
    /// </summary>
    [RPCService]
    public class MSGNodeService
    {
        /// <summary>
        /// 节点性能上报
        /// </summary>
        /// <param name="performaceModel"></param>
        /// <returns></returns>
        public bool Report(PerformaceModel performaceModel)
        {
            Console.WriteLine($"performaceModel:{SAEA.Common.SerializeHelper.Serialize(performaceModel)}");
            return true;
        }
        /// <summary>
        /// 新入的api请求
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public bool AddApi(string url)
        {
            Console.WriteLine("MSGNodeService.AddApi:" + url);
            return true;
        }
        /// <summary>
        /// 获取路由列表
        /// </summary>
        /// <returns></returns>
        public List<RouteInfo> Get()
        {

        }
    }
}
