/****************************************************************************
*项目名称：MicroServiceGateway.Manager.Controllers
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：MicroServiceGateway.Manager.Controllers
*类 名 称：MSGNodeController
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：yswenli@outlook.com
*创建时间：2020/8/21 11:21:21
*描述：
*=====================================================================
*修改时间：2020/8/21 11:21:21
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using MicroServiceGateway.Manager.Attr;
using MicroServiceGateway.Model;
using SAEA.MVC;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroServiceGateway.Manager.Controllers
{
    public class MSGNodeController:Controller
    {
        [Auth(false)]
        public ActionResult Add(string name,string ip,int port)
        {
            var result = new JsonResult<bool>();

            return Json(result);
        }
    }
}
