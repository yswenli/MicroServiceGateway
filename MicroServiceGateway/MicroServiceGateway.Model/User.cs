﻿/****************************************************************************
*项目名称：MicroServiceGateway.Model
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：MicroServiceGateway.Model
*类 名 称：User
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：yswenli@outlook.com
*创建时间：2020/8/21 10:48:46
*描述：
*=====================================================================
*修改时间：2020/8/21 10:48:46
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroServiceGateway.Model
{
    public class User
    {
        public string ID { get; set; }

        public string UserName { get; set; }

        public string NickName { get; set; }

        public string Password { get; set; }

        public Role Role { get; set; }
    }

    public enum Role
    {
        Admin = 1,
        User = 2
    }
}
