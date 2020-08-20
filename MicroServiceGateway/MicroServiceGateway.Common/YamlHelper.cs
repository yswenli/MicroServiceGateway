/****************************************************************************
*项目名称：MicroServiceGateway.Common
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：MicroServiceGateway.Common
*类 名 称：YamlHelper
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：yswenli@outlook.com
*创建时间：2020/8/20 10:31:52
*描述：
*=====================================================================
*修改时间：2020/8/20 10:31:52
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace MicroServiceGateway.Common
{
    /// <summary>
    /// YAML工具类
    /// </summary>
    public static class YamlHelper
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string Serialize<T>(T t)
        {
            var serializer = new SerializerBuilder().Build();

            return serializer.Serialize(t);
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="yaml"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string yaml)
        {
            var deserializer = new DeserializerBuilder()
                                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                                .Build();
            return deserializer.Deserialize<T>(yaml);
        }
    }
}
