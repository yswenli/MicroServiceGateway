/****************************************************************************
*项目名称：MicroServiceGateway.Model
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：MicroServiceGateway.Model
*类 名 称：MSGNodeBaseInfo
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：yswenli@outlook.com
*创建时间：2020/8/21 13:17:53
*描述：
*=====================================================================
*修改时间：2020/8/21 13:17:53
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroServiceGateway.Model
{
    /// <summary>
    /// 微服务网关节点信息
    /// </summary>
    public class MSGNodeBaseInfo
    {
        /// <summary>
        /// 节点ip
        /// </summary>
        public string NodeIP { get; set; } = "127.0.0.1";
        /// <summary>
        /// 节点端口
        /// </summary>
        public int NodePort { get; set; } = 35393;
        /// <summary>
        /// 节点rpc端口
        /// </summary>
        public int NodeRpcPort { get; set; } = 35394;
    }
}
