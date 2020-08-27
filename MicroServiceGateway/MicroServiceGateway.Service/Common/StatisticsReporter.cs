/****************************************************************************
*项目名称：MicroServiceGateway.Service.Common
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：MicroServiceGateway.Service.Common
*类 名 称：StatisticsReporter
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：yswenli@outlook.com
*创建时间：2020/8/27 11:06:16
*描述：
*=====================================================================
*修改时间：2020/8/27 11:06:16
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using MicroServiceGateway.Common;
using MicroServiceGateway.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroServiceGateway.Service.Common
{
    /// <summary>
    /// 统计工具类
    /// </summary>
    public static class StatisticsReporter
    {
        static Performace _performace;

        static PerformaceModel _performaceModel;

        /// <summary>
        /// 资源使用信息
        /// </summary>
        public static PerformaceModel PerformaceModel
        {
            get
            {
                _performaceModel.CPU = _performace.CPU;
                _performaceModel.MemoryUsage = _performace.MemoryUsage;
                _performaceModel.BytesRec = _performace.BytesRec;
                _performaceModel.BytesSen = _performace.BytesSen;
                _performaceModel.TotalThreads = _performace.TotalThreads;
                _performaceModel.HandleCount = _performace.HandleCount;
                return _performaceModel;
            }
        }

        /// <summary>
        /// 统计工具类
        /// </summary>
        static StatisticsReporter()
        {
            var nodeConfig = MSGNodeConfig.Read();

            _performaceModel = new PerformaceModel()
            {
                IP = nodeConfig.NodeIP,
                Port = nodeConfig.NodePort
            };
            PerformanceHelper.OnCounted += PerformanceHelper_OnCounted;
        }

        private static void PerformanceHelper_OnCounted(Performace obj)
        {
            _performace = obj;
        }

        /// <summary>
        /// 启动
        /// </summary>
        public static void Start()
        {
            PerformanceHelper.Start();
        }
        /// <summary>
        /// 停止
        /// </summary>
        public static void Stop()
        {
            PerformanceHelper.Stop();
        }
    }
}
