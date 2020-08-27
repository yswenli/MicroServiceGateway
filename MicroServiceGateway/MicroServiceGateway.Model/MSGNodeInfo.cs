/****************************************************************************
*项目名称：MicroServiceGateway.Model
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：MicroServiceGateway.Model
*类 名 称：MSGNodeInfo
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：yswenli@outlook.com
*创建时间：2020/8/22 16:11:52
*描述：
*=====================================================================
*修改时间：2020/8/22 16:11:52
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using System;

namespace MicroServiceGateway.Model
{
    /// <summary>
    /// 管理端保存的微服务网关节点信息
    /// </summary>
    public class MSGNodeInfo : MSGNodeBaseInfo
    {
        public string NodeName { get; set; }
        /// <summary>
        /// 与管理端连接状态
        /// </summary>
        public bool Linked { get; set; } = false;
        /// <summary>
        /// 与管理端连接时间
        /// </summary>
        public DateTime LinkedTime { get; set; }
    }
}
