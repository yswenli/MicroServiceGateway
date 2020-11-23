using MicroServiceGateway.Manager.Attr;
using MicroServiceGateway.Manager.ServiceDiscovery;
using MicroServiceGateway.Model;
using SAEA.MVC;
using System.Collections.Generic;
using System.Linq;

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
            return Json(new JsonResult<List<string>>().SetResult(MicroServiceCache.GetVirualAddress().ToList()));
        }

        /// <summary>
        /// 获取微服务列表
        /// </summary>
        /// <param name="virtualAddress"></param>
        /// <returns></returns>
        [Auth(false)]
        public ActionResult GetList(string virtualAddress)
        {
            return Json(MicroServiceCache.GetList(virtualAddress));
        }

        /// <summary>
        /// IsOnline
        /// </summary>
        /// <param name="virtualAddress"></param>
        /// <param name="serviceIP"></param>
        /// <param name="servicePort"></param>
        /// <returns></returns>
        public ActionResult IsOnline(string virtualAddress, string serviceIP, int servicePort)
        {
            return Json(MicroServiceCache.GetOnline(virtualAddress, serviceIP, servicePort));
        }


        public ActionResult GetPerformance(string serviceIP, int servicePort)
        {
           return Json(PerformaceModelCache.Get(serviceIP, servicePort));
        }

        /// <summary>
        /// del
        /// </summary>
        /// <param name="virtualAddress"></param>
        /// <param name="serviceIP"></param>
        /// <param name="servicePort"></param>
        /// <returns></returns>
        public ActionResult Del(string virtualAddress, string serviceIP, int servicePort)
        {
            return Json(MicroServiceCache.Del(virtualAddress, serviceIP, servicePort));
        }
    }
}
