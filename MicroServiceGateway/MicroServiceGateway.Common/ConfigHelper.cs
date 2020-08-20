/****************************************************************************
*项目名称：MicroServiceGateway.Common
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：MicroServiceGateway.Common
*类 名 称：ConfigHelper
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：yswenli@outlook.com
*创建时间：2020/8/20 10:36:40
*描述：
*=====================================================================
*修改时间：2020/8/20 10:36:40
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using SAEA.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroServiceGateway.Common
{
    /// <summary>
    /// 本地配置工具类
    /// </summary>
    public static class ConfigHelper
    {

        /// <summary>
        /// 读取配置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static T Read<T>(string fileName)
        {
            var filePath = PathHelper.GetFullName(fileName);

            return YamlHelper.Deserialize<T>(FileHelper.ReadString(filePath));
        }
        /// <summary>
        /// 保存配置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="config"></param>
        /// <param name="fileName"></param>
        public static void Write<T>(T config, string fileName)
        {
            if (config == null) return;

            var filePath = PathHelper.GetFullName(fileName);

            var yaml = YamlHelper.Serialize(config);

            FileHelper.WriteString(filePath, yaml);

        }
    }
}
