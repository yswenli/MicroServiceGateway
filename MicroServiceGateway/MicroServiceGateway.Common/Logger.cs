/****************************************************************************
*项目名称：MicroServiceGateway.Common
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：MicroServiceGateway.Common
*类 名 称：Logger
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：yswenli@outlook.com
*创建时间：2020/8/20 11:16:37
*描述：
*=====================================================================
*修改时间：2020/8/20 11:16:37
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using log4net;
using log4net.Config;
using MicroServiceGateway.Common.Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace MicroServiceGateway.Common
{
    /// <summary>
    /// log4net日志类,
    /// 需要log4net.config文件
    /// </summary>
    public class Logger
    {
        private static ILog _infoLogger;
        private static ILog _warnLogger;
        private static ILog _errorLogger;

        /// <summary>
        /// log4net日志类
        /// </summary>
        static Logger()
        {
            var repository = LogManager.CreateRepository("MicroServiceGateway");

            XmlConfigurator.Configure(repository, new FileInfo("log4net.config"));

            _infoLogger = LogManager.GetLogger(repository.Name, "InfoLogger");

            _warnLogger = LogManager.GetLogger(repository.Name, "WarnLogger");

            _errorLogger = LogManager.GetLogger(repository.Name, "ErrorLogger");
        }

        /// <summary>
        /// 普通日志
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public static void Info(string message, Exception exception = null)
        {
            if (exception == null)
                _infoLogger.Info(message);
            else
                _infoLogger.Info(message, exception);
        }

        /// <summary>
        /// 告警日志
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public static void Warn(string message, Exception exception = null)
        {
            if (exception == null)
                _warnLogger.Warn(message);
            else
                _warnLogger.Warn(message, exception);
        }

        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public static void Error(string message, Exception exception)
        {
            _errorLogger.Error(message, exception);
        }
        /// <summary>
        /// 记录api内容
        /// </summary>
        /// <param name="name"></param>
        /// <param name="url"></param>
        /// <param name="inputs"></param>
        /// <param name="outputs"></param>
        /// <param name="cost"></param>
        /// <param name="exception"></param>
        public static void ApiLog(string name, string url, IEnumerable<object> inputs, IEnumerable<object> outputs, int cost, Exception exception = null)
        {
            var apiLog = new ApiLog()
            {
                name = name,
                url = url,
                inputs = inputs,
                outputs = outputs,
                cost = cost
            };
            var json = JsonConvert.SerializeObject(apiLog);
            if (exception == null)
            {
                Info(json);
            }
            else
            {
                Error(json, exception);
            }
        }
    }

    /// <summary>
    /// 日志类
    /// </summary>
    internal class ApiLog
    {
        public string name { get; set; }

        public string url { get; set; }

        public IEnumerable<object> inputs { get; set; }

        public IEnumerable<object> outputs { get; set; }

        public int cost { get; set; }
    }
}
