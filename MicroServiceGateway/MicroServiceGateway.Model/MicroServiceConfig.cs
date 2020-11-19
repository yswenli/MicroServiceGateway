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
using System.Threading;

namespace MicroServiceGateway.Model
{
    /// <summary>
    /// 微服务配置
    /// </summary>
    public class MicroServiceConfig : MicroServiceBaseInfo
    {
        /// <summary>
        /// 微服务管理地址
        /// </summary>
        public string ManagerServerIP { get; set; } = "127.0.0.1";
        /// <summary>
        /// 微服务管理端口
        /// </summary>
        public int ManagerServerPort { get; set; } = 28080;

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
            try
            {
                return ConfigHelper.Read<MicroServiceConfig>("MicroServiceConfig.yaml");
            }
            catch(Exception ex)
            {
                Thread.Sleep(1000);
                new MicroServiceConfig().Save();
                Logger.Error("MicroServiceConfig.Read", new Exception("加载配置文件MicroServiceConfig.yaml失败", ex));
            }
            return Read();
        }
    }
}
