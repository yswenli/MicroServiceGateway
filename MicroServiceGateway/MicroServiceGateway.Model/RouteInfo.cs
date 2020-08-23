/****************************************************************************
*项目名称：MicroServiceGateway.Model
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：MicroServiceGateway.Model
*类 名 称：RouteInfo
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：yswenli@outlook.com
*创建时间：2020/8/22 11:14:29
*描述：
*=====================================================================
*修改时间：2020/8/22 11:14:29
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using System;

namespace MicroServiceGateway.Model
{
    /// <summary>
    /// 路由信息
    /// </summary>
    public class RouteInfo : MicroServiceBaseInfo
    {
        /// <summary>
        /// 路由权重
        /// </summary>
        public int Score { get; set; } = 100;
        /// <summary>
        /// 熔断状态
        /// </summary>
        public bool Fused { get; set; } = false;
        /// <summary>
        /// 熔断时间
        /// </summary>
        public DateTime FuseTime { get; set; }
    }
}
