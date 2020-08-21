/****************************************************************************
*项目名称：MicroServiceGateway.Model
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：MicroServiceGateway.Model
*类 名 称：MSGNodeConfig
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：yswenli@outlook.com
*创建时间：2020/8/21 13:45:47
*描述：
*=====================================================================
*修改时间：2020/8/21 13:45:47
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using MicroServiceGateway.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroServiceGateway.Model
{
    /// <summary>
    /// 微服务网关节点配置
    /// </summary>
    public class MSGNodeConfig
    {
        /// <summary>
        /// 节点ip
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// 网关端口
        /// </summary>
        public int Port { get; set; } = 39334;


        /// <summary>
        /// 保存配置
        /// </summary>
        public void Save()
        {
            ConfigHelper.Write(this, "MSGNodeConfig.yaml");
        }

        /// <summary>
        /// 读取管理端配置
        /// </summary>
        /// <returns></returns>
        public static MSGNodeConfig Read()
        {
            return ConfigHelper.Read<MSGNodeConfig>("MSGNodeConfig.yaml");
        }
    }
}
