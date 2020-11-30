/****************************************************************************
*项目名称：MicroServiceGateway.Calllogger
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：MicroServiceGateway.Calllogger
*类 名 称：CallLog
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：yswenli@outlook.com
*创建时间：2020/8/25 14:37:04
*描述：
*=====================================================================
*修改时间：2020/8/25 14:37:04
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using MicroServiceGateway.Common;
using MicroServiceGateway.Model;
using SAEA.Common;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MicroServiceGateway.Calllogger
{
    /// <summary>
    /// CallLog
    /// </summary>
    public static class CallLog
    {
        static ConcurrentBag<ApiLog> _bags;

        static ConcurrentDictionary<string, long> _staticsDic;

        /// <summary>
        /// CallLog
        /// </summary>
        static CallLog()
        {
            _bags = new ConcurrentBag<ApiLog>();

            _staticsDic = new ConcurrentDictionary<string, long>();

            //每天清理一次数据
            Task.Factory.StartNew(() =>
            {
                var date = DateTimeHelper.Now.Date;

                while (true)
                {
                    Thread.Sleep(1000);

                    if (DateTimeHelper.Now >= date)
                    {
                        date = date.AddDays(1);

                        _staticsDic.Clear();
                    }
                }
            }, TaskCreationOptions.LongRunning);
        }

        /// <summary>
        /// 获取url
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        static string GetUrl(Uri uri)
        {
            var url = uri.AbsoluteUri;

            var offset = url.IndexOf("?");

            if (offset > 0)
            {
                url = url.Substring(offset);
            }
            return url.ToLower();
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="name"></param>
        /// <param name="uri"></param>
        /// <param name="inputs"></param>
        /// <param name="output"></param>
        /// <param name="cost"></param>
        /// <param name="exception"></param>
        public static void Log(string name, Uri uri, IEnumerable<KeyValuePair<string, string>> inputs, object output, long cost, Exception exception = null)
        {
            var url = GetUrl(uri);

            _staticsDic.AddOrUpdate(url, 1, (k, v) => v++);

            var apiLog = new ApiLog()
            {
                name = name,
                url = uri.AbsoluteUri,
                inputs = inputs,
                output = output,
                cost = cost
            };
            _bags.Add(apiLog);
        }

        /// <summary>
        /// 读取日志
        /// </summary>
        /// <returns></returns>
        public static List<string> GetApiLogs()
        {
            if (_bags.IsEmpty)
            {
                return new List<string>();
            }
            List<string> data = new List<string>();
            while (_bags.TryTake(out ApiLog apiLog))
            {
                data.Add(SerializeHelper.Serialize(apiLog));
            }
            return data;
        }

        /// <summary>
        /// 获取api统计
        /// </summary>
        /// <returns></returns>
        public static List<Apistatistical> GetApiStatics()
        {
            var apiStatics = new List<Apistatistical>();

            foreach (var item in _staticsDic)
            {
                apiStatics.Add(new Apistatistical()
                {
                    Url = item.Key,
                    Count = item.Value
                });
            }

            return apiStatics;
        }

    }
}
