/****************************************************************************
*项目名称：MicroServiceGateway.Service
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：MicroServiceGateway.Service
*类 名 称：ForwardAttribute
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：yswenli@outlook.com
*创建时间：2020/8/21 14:12:17
*描述：
*=====================================================================
*修改时间：2020/8/21 14:12:17
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using SAEA.MVC;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroServiceGateway.Service
{
    public class ForwardAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionResult result)
        {
            throw new NotImplementedException();
        }

        public override bool OnActionExecuting()
        {
            throw new NotImplementedException();
        }
    }
}
