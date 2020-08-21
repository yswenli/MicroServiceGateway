/****************************************************************************
*项目名称：MicroServiceGateway.Service.Controllers
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：MicroServiceGateway.Service.Controllers
*类 名 称：ForwardingServiceController
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：yswenli@outlook.com
*创建时间：2020/8/21 14:10:14
*描述：
*=====================================================================
*修改时间：2020/8/21 14:10:14
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using SAEA.MVC;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroServiceGateway.Service.Controllers
{
    /// <summary>
    /// 微服务网关转发控制器
    /// </summary>
    public class ForwardingServiceController : Controller
    {
        /// <summary>
        /// 转发
        /// </summary>
        /// <returns></returns>
        [Forward]
        public ActionResult Forward()
        {
            return Empty();
        }
    }
}
