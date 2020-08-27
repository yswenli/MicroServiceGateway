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
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace MicroServiceGateway.Calllogger
{
    /// <summary>
    /// CallLog
    /// </summary>
    public static class CallLog
    {
        static ConcurrentQueue<ApiLog> _queue;

        /// <summary>
        /// CallLog
        /// </summary>
        static CallLog()
        {
            _queue = new ConcurrentQueue<ApiLog>();
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="name"></param>
        /// <param name="url"></param>
        /// <param name="inputs"></param>
        /// <param name="output"></param>
        /// <param name="cost"></param>
        /// <param name="exception"></param>
        public static void Log(string name, string url, IEnumerable<KeyValuePair<string,string>> inputs, object output, long cost, Exception exception = null)
        {
            var apiLog = new ApiLog()
            {
                name = name,
                url = url,
                inputs = inputs,
                output = output,
                cost = cost
            };
            _queue.Enqueue(apiLog);
        }

        /// <summary>
        /// 阻塞式读取日志
        /// </summary>
        /// <returns></returns>
        public static ApiLog Read()
        {
            ApiLog apiLog;

            while (_queue.IsEmpty || !_queue.TryDequeue(out apiLog) || apiLog == null)
            {
                Thread.Sleep(1);
            }
            return apiLog;
        }

    }
}
