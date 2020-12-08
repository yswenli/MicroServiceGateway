/****************************************************************************
*项目名称：MicroServiceGateway.Client
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：MicroServiceGateway.Client
*类 名 称：MSNMService
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：yswenli@outlook.com
*创建时间：2020/12/8 18:21:08
*描述：
*=====================================================================
*修改时间：2020/12/8 18:21:08
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using MicroServiceGateway.Client.Consumer;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace MicroServiceGateway.Client
{
    public static class MSNMService
    {
        static RPCServiceProxy _service;

        public static void Init(string url)
        {
            _service = new RPCServiceProxy(url);
        }

        public static RPCServiceProxy GetService()
        {
            return _service;
        }
    }
}
