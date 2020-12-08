/****************************************************************************
*项目名称：MicroServiceGateway.Client
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：MicroServiceGateway.Client
*类 名 称：AppConfigHelper
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：yswenli@outlook.com
*创建时间：2020/12/8 18:07:49
*描述：
*=====================================================================
*修改时间：2020/12/8 18:07:49
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using MicroServiceGateway.Model;
using SAEA.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroServiceGateway.Client
{
    internal static class AppConfigHelper
    {
        public static List<ConfigDataItem> GetConfigs(string appId, string env)
        {
            var list= MSNMService.GetService().MSGClientService.GetConfigs(appId, env);

            if (list == null)
            {
                return null;
            }
            else
            {
                return list.ConvertToList<ConfigDataItem>();
            }
        }
    }
}
