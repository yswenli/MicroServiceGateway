/****************************************************************************
*项目名称：MicroServiceGateway.Model
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：MicroServiceGateway.Model
*类 名 称：MicroServiceConfig
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：yswenli@outlook.com
*创建时间：2020/8/20 9:38:09
*描述：
*=====================================================================
*修改时间：2020/8/20 9:38:09
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using MicroServiceGateway.Common;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace MicroServiceGateway.Model
{
    /// <summary>
    /// 微服务配置
    /// </summary>
    public class MicroServiceConfig
    {
        /// <summary>
        /// 微服务管理地址
        /// </summary>
        public string ManagerUrl { get; set; }

        /// <summary>
        /// 网关转发地址
        /// </summary>
        public string VirtualAddress { get; set; }

        /// <summary>
        /// 服务环境
        /// </summary>
        public string Env { get; set; } = "dev";
        /// <summary>
        /// 服务名称
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// 服务端口
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 服务健康检测地址
        /// </summary>
        public string HealthChecksUrl { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 保存
        /// </summary>
        public void Save()
        {
            ConfigHelper.Write(this, "MicroServiceConfig.yaml");
        }

        /// <summary>
        /// 读取
        /// </summary>
        /// <returns></returns>
        public static MicroServiceConfig Read()
        {
            return ConfigHelper.Read<MicroServiceConfig>("MicroServiceConfig.yaml");
        }
    }
}
