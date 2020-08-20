/****************************************************************************
*项目名称：MicroServiceGateway.Common
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：MicroServiceGateway.Common
*类 名 称：PerformanceHelper
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：yswenli@outlook.com
*创建时间：2020/8/20 13:54:23
*描述：
*=====================================================================
*修改时间：2020/8/20 13:54:23
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;

namespace MicroServiceGateway.Common
{
    /// <summary>
    /// PerformanceUtil
    /// </summary>
    public static class PerformanceHelper
    {
        static Process _currentProcess;

        static string _instanceName;

        static PerformanceCounter _cpuCounter;

        static PerformanceCounter _privateMemCounter;

        static PerformanceCounter _threadCounter;

        static PerformanceCounter _handlerCounter;

        static List<PerformanceCounter>[] _pcss;

        static int _cpuCount = 0;

        /// <summary>
        /// OnCounted
        /// </summary>
        public static event Action<Performace> OnCounted;

        static PerformanceHelper()
        {
            //Process.Start("LODCTR /R");

            _currentProcess = Process.GetCurrentProcess();

            _instanceName = GetInstanceName();

            _cpuCount = Environment.ProcessorCount;

            _cpuCounter = new PerformanceCounter("Process", "% Processor Time", _instanceName);

            _cpuCounter.NextValue();

            _privateMemCounter = new PerformanceCounter("Process", "Working Set - Private", _instanceName);

            _threadCounter = new PerformanceCounter("Process", "Thread Count", _instanceName);

            _handlerCounter = new PerformanceCounter("Process", "Handle Count", _instanceName);


            List<PerformanceCounter> pcs = new List<PerformanceCounter>();
            List<PerformanceCounter> pcs2 = new List<PerformanceCounter>();
            string[] names = GetAdapter();
            foreach (string name in names)
            {
                try
                {
                    PerformanceCounter pc = new PerformanceCounter("Network Interface", "Bytes Received/sec", name.Replace('(', '[').Replace(')', ']'), ".");
                    PerformanceCounter pc2 = new PerformanceCounter("Network Interface", "Bytes Sent/sec", name.Replace('(', '[').Replace(')', ']'), ".");
                    pc.NextValue();
                    pcs.Add(pc);
                    pcs2.Add(pc2);
                }
                catch
                {
                    continue;
                }
            }
            _pcss = new List<PerformanceCounter>[2];
            _pcss[0] = pcs;
            _pcss[1] = pcs2;
        }

        static bool IsUnix()
        {
            return Environment.OSVersion.Platform == PlatformID.Unix || Environment.OSVersion.Platform == PlatformID.MacOSX;
        }

        static string GetInstanceName()
        {
            return IsUnix() ? string.Format("{0}/{1}", _currentProcess.Id, _currentProcess.ProcessName) : _currentProcess.ProcessName;
        }

        static string[] GetAdapter()
        {
            NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
            string[] name = new string[adapters.Length];
            int index = 0;
            foreach (NetworkInterface ni in adapters)
            {
                name[index] = ni.Description;
                index++;
            }
            return name;
        }

        static bool _running = false;

        /// <summary>
        /// Start
        /// </summary>
        public static void Start()
        {
            if (!_running)
            {
                _running = true;

                Task.Factory.StartNew(() =>
                {
                    while (_running)
                    {
                        Thread.Sleep(1000);

                        float recv = 0;
                        float sent = 0;

                        foreach (PerformanceCounter pc in _pcss[0])
                        {
                            recv += pc.NextValue();
                        }
                        foreach (PerformanceCounter pc in _pcss[1])
                        {
                            sent += pc.NextValue();
                        }

                        var performace = new Performace()
                        {
                            CPU = _cpuCounter.NextValue() / _cpuCount,
                            MemoryUsage = _privateMemCounter.NextValue() / 1024,
                            TotalThreads = _threadCounter.NextValue(),
                            HandleCount = _handlerCounter.NextValue(),
                            BytesRec = recv,
                            BytesSen = sent
                        };

                        OnCounted.BeginInvoke(performace, null, null);
                    }
                }, TaskCreationOptions.LongRunning);
            }
        }

        /// <summary>
        /// Stop
        /// </summary>
        public static void Stop()
        {
            _running = false;
        }

    }

    /// <summary>
    /// 性能统计项
    /// </summary>
    public class Performace
    {
        /// <summary>
        /// CPU
        /// </summary>
        public float CPU { get; set; }

        /// <summary>
        /// MemoryUsage
        /// </summary>
        public float MemoryUsage { get; set; }
        /// <summary>
        /// TotalThreads
        /// </summary>
        public float TotalThreads { get; set; }
        /// <summary>
        /// HandleCount
        /// </summary>
        public float HandleCount { get; set; }
        /// <summary>
        /// BytesRec
        /// </summary>
        public float BytesRec { get; set; }
        /// <summary>
        /// BytesSen
        /// </summary>
        public float BytesSen { get; set; }
    }
}
