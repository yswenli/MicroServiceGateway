/****************************************************************************
*项目名称：MicroServiceGateway.Common
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：MicroServiceGateway.Common
*类 名 称：StringFormatExtention
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：yswenli@outlook.com
*创建时间：2020/8/24 10:03:55
*描述：
*=====================================================================
*修改时间：2020/8/24 10:03:55
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using SAEA.Common;
using System;
using System.Text.RegularExpressions;

namespace MicroServiceGateway.Common
{
    /// <summary>
    /// 这符串格式
    /// </summary>
    public static class StringFormatExtention
    {
        static Regex regex = new Regex("^[0-9a-zA-Z_]{1,20}$", RegexOptions.Compiled);

        /// <summary>
        /// 是否符合数字、字母、下划线，最长不超过20字规定
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool VirtualAddressValid(this string str)
        {
            return regex.IsMatch(str);
        }

        /// <summary>
        /// 解析节点获取的url
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static Tuple<string, string> ToVirtualAddressUrl(this string url)
        {
            if (!string.IsNullOrEmpty(url) && url.IndexOf("/") > -1)
            {
                var auri = new Uri(url).AbsoluteUri;

                var arr = auri.Split("/", StringSplitOptions.RemoveEmptyEntries);
                
                return Tuple.Create(arr[0], auri.Substring(auri.IndexOf(arr[0]) + arr[0].Length));
            }
            return null;
        }

        
    }
}
