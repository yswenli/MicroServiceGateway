/****************************************************************************
*项目名称：MicroServiceGateway.Model
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：MicroServiceGateway.Model
*类 名 称：ManagerConfig
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：yswenli@outlook.com
*创建时间：2020/8/20 17:44:36
*描述：
*=====================================================================
*修改时间：2020/8/20 17:44:36
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using MicroServiceGateway.Common;
using System;

namespace MicroServiceGateway.Model
{
    /// <summary>
    /// 微服务管理端配置
    /// </summary>
    public class ManagerConfig
    {
        /// <summary>
        /// redis 连接字符串
        /// </summary>
        public string RedisCnnStr { get; set; } = "server=127.0.0.1:6379;passwords=yswenli";

        /// <summary>
        /// 保存配置
        /// </summary>
        public void Save()
        {
            ConfigHelper.Write(this, "ManagerConfig.yaml");
        }

        /// <summary>
        /// 读取管理端配置
        /// </summary>
        /// <returns></returns>
        public static ManagerConfig Read()
        {
            ManagerConfig mc = null;
            Exception ex = null;
            try
            {
                mc = ConfigHelper.Read<ManagerConfig>("ManagerConfig.yaml");
            }
            catch (Exception e)
            {
                ex = e;
            }
            try
            {
                if (ex != null)
                {
                    new ManagerConfig().Save();
                }
            }
            catch
            {

            }
            if (ex != null)
            {
                throw new Exception("初始化配置有误，请检查ManagerConfig.yaml文件及其内容", ex);
            }
            return mc;
        }
    }
}
