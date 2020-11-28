using MicroServiceGateway.Model;
using System.Collections.Concurrent;

namespace MicroServiceGateway.Manager.Libs
{
    /// <summary>
    /// 网关服务资源使用情况缓存
    /// </summary>
    public static class MsgNodePerformanceCache
    {
        static ConcurrentDictionary<string, PerformaceModel> _msgNodePerformanceCache;

        /// <summary>
        /// 网关服务资源使用情况缓存
        /// </summary>
        static MsgNodePerformanceCache()
        {
            _msgNodePerformanceCache = new ConcurrentDictionary<string, PerformaceModel>();
        }

        /// <summary>
        /// Set
        /// </summary>
        /// <param name="nodeName"></param>
        /// <param name="performaceModel"></param>
        public static void Set(string nodeName, PerformaceModel performaceModel)
        {
            _msgNodePerformanceCache.AddOrUpdate(nodeName, performaceModel, (k, v) => performaceModel);
        }

        /// <summary>
        /// Get
        /// </summary>
        /// <param name="nodeName"></param>
        /// <returns></returns>
        public static PerformaceModel Get(string nodeName)
        {
            if (_msgNodePerformanceCache.TryGetValue(nodeName, out PerformaceModel performaceModel))
            {
                if (performaceModel != null)
                {
                    return performaceModel;
                }
            }
            return new PerformaceModel();
        }

        /// <summary>
        /// Del
        /// </summary>
        /// <param name="nodeName"></param>
        public static void Del(string nodeName)
        {
            _msgNodePerformanceCache.TryRemove(nodeName, out PerformaceModel _);
        }

    }
}
