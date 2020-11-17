/****************************************************************************
*项目名称：MicroServiceGateway.Model
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：MicroServiceGateway.Model
*类 名 称：PerformaceModel
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：yswenli@outlook.com
*创建时间：2020/8/20 15:16:12
*描述：
*=====================================================================
*修改时间：2020/8/20 15:16:12
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using MicroServiceGateway.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroServiceGateway.Model
{
    public class PerformaceModel : Performace
    {
        public string VirtualAddress { get; set; }
        public string IP { get; set; }
        public int Port { get; set; }

        public DateTime Created { get; set; }
    }
}
