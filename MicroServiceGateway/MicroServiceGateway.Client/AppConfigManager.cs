/****************************************************************************
*项目名称：MicroServiceGateway.Client
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：MicroServiceGateway.Client
*类 名 称：AppConfigManager
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：yswenli@outlook.com
*创建时间：2020/12/8 17:55:45
*描述：
*=====================================================================
*修改时间：2020/12/8 17:55:45
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using System.Collections.Concurrent;
using System.Linq;

namespace MicroServiceGateway.Client
{
    /// <summary>
    /// AppConfigManager
    /// </summary>
    public sealed class AppConfigManager
    {
        static object _syncObject = new object();

        static AppConfigManager _appConfigManager = null;

        ConcurrentDictionary<string, AppConfig> _cache;

        /// <summary>
        /// AppConfigManager
        /// </summary>
        public static AppConfigManager Instance
        {
            get
            {
                if (_appConfigManager == null)
                {
                    lock (_syncObject)
                    {
                        if (_appConfigManager == null)
                        {
                            _appConfigManager = new AppConfigManager();
                        }
                    }
                }
                return _appConfigManager;
            }
        }

        internal AppConfigManager()
        {
            _cache = new ConcurrentDictionary<string, AppConfig>();
        }

        object _locker = new object();

        public AppConfig this[string appId]
        {
            get
            {
                AppConfig result;
                if (!_cache.ContainsKey(appId))
                {
                    lock (_locker)
                    {
                        if (!_cache.ContainsKey(appId))
                        {
                            result = new AppConfig(appId);
                            _cache.TryAdd(appId, result);
                        }
                        else
                        {
                            result = _cache[appId];
                        }
                    }
                }
                else
                {
                    result = _cache[appId];
                }
                return result;
            }
        }

        public class AppConfig
        {
            string _appId;

            ConcurrentDictionary<string, NameValue> _cache;

            public AppConfig(string appId)
            {
                _appId = appId;
                _cache = new ConcurrentDictionary<string, NameValue>();
            }

            public NameValue this[string env]
            {
                get
                {
                    var key = $"{_appId}_{env}";

                    if (_cache.ContainsKey(key))
                    {
                        return _cache[key];
                    }
                    else
                    {
                        var nameValue = new NameValue(_appId, env);

                        _cache.TryAdd(key, nameValue);
                      
                        return nameValue;
                    }
                }
            }


            public class NameValue
            {
                ConcurrentDictionary<string, string> _data;

                string _appId, _env;

                public NameValue(string appId, string env)
                {
                    _appId = appId;

                    _env = env;

                    _data = new ConcurrentDictionary<string, string>();

                    var list = AppConfigHelper.GetConfigs(_appId, _env);

                    if (list != null && list.Any())
                    {
                        foreach (var item in list)
                        {
                            _data.TryAdd($"{_appId}_{_env}_{item.Name}", item.Value);
                        }
                    }
                }

                public string this[string name]
                {
                    get
                    {
                        string val;
                        _data.TryGetValue($"{_appId}_{_env}_{name}", out val);
                        return val;
                    }
                }
            }

        }
    }
}
