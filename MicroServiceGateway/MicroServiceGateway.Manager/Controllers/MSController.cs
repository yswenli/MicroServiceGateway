using MicroServiceGateway.Manager.Attr;
using MicroServiceGateway.Manager.ServiceDiscovery;
using MicroServiceGateway.Model;
using SAEA.MVC;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroServiceGateway.Manager.Controllers
{
    /// <summary>
    /// 微服务
    /// </summary>
    public class MSController : Controller
    {
        /// <summary>
        /// 获取虚拟地址列表
        /// </summary>
        /// <returns></returns>
        [Auth(false)]
        public ActionResult GetVirtualAddress()
        {
            return Json(new JsonResult<List<string>>().SetResult(MicroServiceCache.GetVirualAddress()));
        }

        /// <summary>
        /// 获取微服务列表
        /// </summary>
        /// <param name="virtualAddress"></param>
        /// <returns></returns>
        [Auth(false)]
        public ActionResult GetList(string virtualAddress)
        {
            return Json(new JsonResult<List<MicroServiceConfig>>().SetResult(MicroServiceCache.GetList(virtualAddress)));
        }
    }
}
