/****************************************************************************
*项目名称：MicroServiceGateway.Model
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：MicroServiceGateway.Model
*类 名 称：MicroServiceBaseInfo
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：yswenli@outlook.com
*创建时间：2020/8/22 15:14:30
*描述：
*=====================================================================
*修改时间：2020/8/22 15:14:30
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/

namespace MicroServiceGateway.Model
{
    /// <summary>
    /// 微服务信息
    /// </summary>
    public class MicroServiceBaseInfo
    {
        /// <summary>
        /// 集群
        /// </summary>
        public string Cluster { get; set; } = "cluster1";
        /// <summary>
        /// 环境
        /// </summary>
        public string Env { get; set; } = "dev";
        /// <summary>
        /// 服务名称
        /// </summary>
        public string ServiceName { get; set; } = "APIService1";
        /// <summary>
        /// 虚拟地址
        /// </summary>
        public string VirtualAddress { get; set; } = "APIService";
        /// <summary>
        /// 服务ip
        /// </summary>
        public string ServiceIP { get; set; } = "127.0.0.1";
        /// <summary>
        /// 服务端口
        /// </summary>
        public int ServicePort { get; set; } = 5000;
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; } = "APIService1 is a test";
    }
}
