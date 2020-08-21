/****************************************************************************
*项目名称：MicroServiceGateway.Service.RPCSerivices
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：MicroServiceGateway.Service.RPCSerivices
*类 名 称：MSGNodeService
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：yswenli@outlook.com
*创建时间：2020/8/21 13:40:40
*描述：
*=====================================================================
*修改时间：2020/8/21 13:40:40
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using MicroServiceGateway.Model;
using SAEA.RPC;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroServiceGateway.Service.RPCSerivices
{
    [RPCService]
    public class MSGNodeService
    {
        public PerformaceModel GetReport()
        {
            return new PerformaceModel()
            {

            };
        }
    }
}
