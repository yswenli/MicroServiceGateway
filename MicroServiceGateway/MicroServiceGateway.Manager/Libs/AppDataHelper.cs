using MicroServiceGateway.Data.Redis;
using MicroServiceGateway.Model;
using SAEA.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MicroServiceGateway.Manager.Libs
{
    public static class AppDataHelper
    {
        #region AppData

        public static JsonResult<string> SetAppData(string appId, string appName)
        {
            JsonResult<string> result = JsonResult<string>.Default;

            if (string.IsNullOrEmpty(appId))
            {
                var appData = AppDataOperation.Get(appId);

                if (appData == null)
                {
                    appData = new AppData();

                    appData.ID = IdWorker.GetId().ToString();

                    appData.Envirments = Enum.GetNames(typeof(AppDataEnvirment)).ToList();

                    appData.Created = DateTimeHelper.Now.ToFString();

                    foreach (var item in appData.Envirments)
                    {
                        ConfigDataOperation.Set(new ConfigData()
                        {
                            AppID = appData.ID,
                            ID = IdWorker.GetId().ToString(),
                            Envirment = item,
                            Data = new List<ConfigDataItem>()
                        });
                    }
                }

                appData.Name = appName;

                appData.Updated = DateTimeHelper.Now.ToFString();

                AppDataOperation.Set(appData);

                result.SetResult("OK");
            }
            else
            {
                var appData = new AppData();

                appData.ID = IdWorker.GetId().ToString();

                appData.Envirments = Enum.GetNames(typeof(AppDataEnvirment)).ToList();

                appData.Created = DateTimeHelper.Now.ToFString();

                appData.Name = appName;

                appData.Updated = DateTimeHelper.Now.ToFString();

                AppDataOperation.Set(appData);

                foreach (var item in appData.Envirments)
                {
                    ConfigDataOperation.Set(new ConfigData()
                    {
                        AppID = appData.ID,
                        ID = IdWorker.GetId().ToString(),
                        Envirment = item,
                        Data = new List<ConfigDataItem>()
                    });
                }

                result.SetResult("OK");
            }

            return result;
        }


        public static JsonResult<List<AppData>> GetAppDataList()
        {
            JsonResult<List<AppData>> result = JsonResult<List<AppData>>.Default;

            var list = AppDataOperation.GetList();

            if (list != null && list.Any())
            {
                result.SetResult(list);
            }
            else
            {
                result.SetError("获取appdata列表失败");
            }

            return result;
        }
        #endregion

        #region ConfigData

        public static JsonResult<ConfigData> GetConfigData(string appId, string env)
        {
            JsonResult<ConfigData> result = JsonResult<ConfigData>.Default;

            result.SetResult(ConfigDataOperation.Get(appId, env));

            return result;
        }

        #endregion

        #region ConfigDataItem

        public static event Action<List<ConfigDataItem>> OnChanged;

        public static JsonResult<string> SetConfigDataItem(string configDataId, string id, string name, string value)
        {
            JsonResult<string> result = JsonResult<string>.Default;

            var item = ConfigDataItemOperation.Get(configDataId, id);

            if (item == null)
            {
                item = new ConfigDataItem()
                {
                    ConfigDataID = configDataId,
                    ID = IdWorker.GetId().ToString(),
                    OperationMode = 1,
                    Created = DateTimeHelper.Now.ToFString()
                };
            }
            else
            {
                item.OperationMode = 2;
            }
            item.Name = name;
            item.Value = value;
            item.Updated = DateTimeHelper.Now.ToFString();
            item.IsPublished = false;
            item.Published = string.Empty;
            ConfigDataItemOperation.Set(item);

            result.SetResult("OK");

            return result;
        }


        public static JsonResult<List<ConfigDataItem>> GetConfigDataItems(string configDataId)
        {
            JsonResult<List<ConfigDataItem>> result = JsonResult<List<ConfigDataItem>>.Default;

            result.SetResult(ConfigDataItemOperation.GetList(configDataId));

            return result;
        }

        public static JsonResult<bool> RemoveConfigDataItem(string configDataId, string id)
        {
            JsonResult<bool> result = JsonResult<bool>.Default;

            var item = ConfigDataItemOperation.Get(configDataId, id);

            if (item != null)
            {
                item.OperationMode = -1;
                item.IsPublished = false;
                item.Published = string.Empty;
                ConfigDataItemOperation.Set(item);
                result.SetResult(true);
            }

            return result;
        }

        public static JsonResult<bool> PublishConfig(string configDataId, string ids)
        {
            JsonResult<bool> result = JsonResult<bool>.Default;

            if (!string.IsNullOrEmpty(configDataId) && !string.IsNullOrEmpty(ids))
            {
                var list = ConfigDataItemOperation.GetList(configDataId);

                if (list != null)
                {
                    var idArr = ids.Split(",");

                    var clts = list.Where(b => idArr.Contains(b.ID)).ToList();

                    if (clts != null && clts.Any())
                    {
                        foreach (var item in clts)
                        {
                            if (item.OperationMode == -1)
                            {
                                ConfigDataItemOperation.Remove(item);
                                list.Remove(item);
                            }
                            else
                            {
                                item.IsPublished = true;
                                item.Updated = item.Published = DateTimeHelper.Now.ToFString();
                                ConfigDataItemOperation.Set(item);
                            }
                        }

                        OnChanged?.Invoke(clts);
                    }
                    result.SetResult(true);
                }
                else
                {
                    result.SetError("操作失败");
                }
            }
            else
            {
                result.SetError("操作失败");
            }
            return result;
        }


        public static List<ConfigDataItem> GetConfigs(string appIds, string env)
        {
            if (string.IsNullOrEmpty(appIds) || string.IsNullOrEmpty(env)) return new List<ConfigDataItem>();

            var configData = ConfigDataOperation.Get(appIds, env);

            if (configData != null && !string.IsNullOrEmpty(configData.ID))
            {
                return ConfigDataItemOperation.GetList(configData.ID);
            }
            return new List<ConfigDataItem>();
        }
        #endregion
    }
}
