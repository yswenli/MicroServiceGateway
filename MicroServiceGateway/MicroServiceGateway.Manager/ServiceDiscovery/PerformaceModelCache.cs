/****************************************************************************
*项目名称：MicroServiceGateway.Manager.Libs
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：MicroServiceGateway.Manager.Libs
*类 名 称：PerformaceModelCache
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：yswenli@outlook.com
*创建时间：2020/8/25 15:58:30
*描述：
*=====================================================================
*修改时间：2020/8/25 15:58:30
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using MicroServiceGateway.Model;
using SAEA.Common;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace MicroServiceGateway.Manager.ServiceDiscovery
{
    /// <summary>
    /// 资源情况缓存
    /// </summary>
    public class PerformaceModelCache
    {
        static ConcurrentDictionary<string, PerformaceModel> _cache;

        /// <summary>
        /// 资源情况缓存
        /// </summary>
        static PerformaceModelCache()
        {
            _cache = new ConcurrentDictionary<string, PerformaceModel>();

            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    if (!_cache.IsEmpty)
                    {
                        var values = _cache.Values;

                        foreach (var value in values)
                        {
                            if (value.Created.AddMinutes(1) <= DateTimeHelper.Now)
                            {
                                Remove(value);
                            }
                        }
                    }

                    Thread.Sleep(30 * 1000);
                }
            });
        }

        static string GetKey(PerformaceModel performaceModel)
        {
            return $"{performaceModel.IP}_{performaceModel.Port}";
        }

        /// <summary>
        /// 更新资源情况
        /// </summary>
        /// <param name="performaceModel"></param>
        public static void Set(PerformaceModel performaceModel)
        {
            _cache.AddOrUpdate(GetKey(performaceModel), performaceModel, (k, v) => performaceModel);
        }

        /// <summary>
        /// 获取资源情况
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public static PerformaceModel Get(string ip, int port)
        {
            if (_cache.TryGetValue($"{ip}_{port}", out PerformaceModel performaceModel))
            {
                return performaceModel;
            }
            return null;
        }

        /// <summary>
        /// 移除资源情况
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        public static void Remove(PerformaceModel performaceModel)
        {
            _cache.TryRemove(GetKey(performaceModel), out PerformaceModel t);
        }

    }
}
