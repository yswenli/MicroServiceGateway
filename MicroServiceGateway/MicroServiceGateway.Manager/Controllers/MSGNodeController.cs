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
using MicroServiceGateway.Common;
using MicroServiceGateway.Data.Redis;
using MicroServiceGateway.Manager.Attr;
using MicroServiceGateway.Manager.Consumer;
using MicroServiceGateway.Manager.Libs;
using MicroServiceGateway.Model;
using SAEA.MVC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroServiceGateway.Manager.Controllers
{
    /// <summary>
    /// 微服务节点
    /// </summary>
    public class MSGNodeController : Controller
    {
        /// <summary>
        /// 添加微服务节点
        /// </summary>
        /// <param name="nodeName"></param>
        /// <param name="nodeIP"></param>
        /// <param name="nodePort"></param>
        /// <param name="nodeRpcPort"></param>
        /// <returns></returns>
        [Auth(false)]
        public ActionResult Add(string nodeName, string nodeIP, int nodePort = 0, int nodeRpcPort = 0)
        {
            var result = JsonResult<bool>.Default;

            if (string.IsNullOrEmpty(nodeName))
            {
                result.SetError(new Exception("nodeName 不能为空"));
                return Json(result);
            }

            if (string.IsNullOrEmpty(nodeIP))
            {
                result.SetError(new Exception("nodeIP 不能为空"));
                return Json(result);
            }

            try
            {
                var msgnode = new MSGNodeInfo()
                {
                    NodeName = nodeName,
                    NodeIP = nodeIP,
                    NodePort = nodePort,
                    NodeRpcPort = nodeRpcPort
                };
                if (MSGNodeOperation.Exists(msgnode.NodeName))
                {
                    result.SetError(new Exception("微服务节点nodeName已存在"));
                    return Json(result);
                }
                try
                {
                    RPCServiceProxy serviceProxy = new RPCServiceProxy($"rpc://{nodeIP}:{nodeRpcPort}");

                    var performace = serviceProxy.NodeService.GetPerformace();

                    if (performace != null)
                    {
                        msgnode.Linked = true;
                        msgnode.LinkedTime = DateTime.Now;

                        MsgNodeRpcServiceCache.Set(msgnode.NodeName, serviceProxy);

                        result.SetResult(true);
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error("添加微服务节点失败", ex);
                    result.SetError(new Exception("微服务节点连接失败"));
                }
                finally
                {
                    MSGNodeOperation.Set(msgnode);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("添加微服务节点失败", ex);
                result.SetError(new Exception("添加微服务节点失败"));
            }

            return Json(result);
        }

        /// <summary>
        /// 删除微服务节点
        /// </summary>
        /// <param name="nodeName"></param>
        /// <returns></returns>
        public ActionResult Del(string nodeName)
        {
            var result = JsonResult<bool>.Default;

            try
            {
                MSGNodeOperation.Del(nodeName);

                MsgNodeRpcServiceCache.Del(nodeName);

                result.SetResult(true);
            }
            catch (Exception ex)
            {
                Logger.Error("删除微服务节点失败", ex);
                result.SetError(new Exception("删除微服务节点失败"));
            }
            return Json(result);
        }

        /// <summary>
        /// 获取微服务网关列表
        /// </summary>
        /// <returns></returns>
        [Auth(false)]
        public ActionResult GetList()
        {
            var result = JsonResult<List<MSGNodeInfo>>.Default;

            try
            {
                var list = MSGNodeOperation.GetList().ToList();

                result.SetResult(list);
            }
            catch (Exception ex)
            {
                Logger.Error("获取微服务节点列表失败", ex);
                result.SetError(new Exception("获取微服务节点列表失败"));
            }
            return Json(result);
        }


        /// <summary>
        /// 获取微服务网关信息
        /// </summary>
        /// <param name="nodeName"></param>
        /// <returns></returns>
        [Auth(false)]
        public ActionResult GetPerformance(string nodeName)
        {
            var result = JsonResult<PerformaceModel>.Default;

            try
            {
                result.SetResult(MsgNodePerformanceCache.Get(nodeName));
            }
            catch (Exception ex)
            {
                Logger.Error("获取微服务节点信息失败", ex);
                result.SetError(new Exception("获取微服务节点信息失败"));
            }
            return Json(result);
        }


        /// <summary>
        /// 获取网关节点信息集合
        /// </summary>
        /// <returns></returns>
        [Auth(false)]
        public ActionResult GetConfig()
        {
            var result = JsonResult<string>.Default;

            try
            {
                var list = MSGNodeOperation.GetList().ToList();

                result.SetResult("OK", Serialize(list, true));
            }
            catch (Exception ex)
            {
                Logger.Error("获取微服务节点列表失败", ex);
                result.SetError(new Exception("获取微服务节点列表失败"));
            }
            return Json(result);
        }

        /// <summary>
        /// 将网关节点信息添加到集合
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [Auth(false)]
        public ActionResult SetConfig(string json)
        {
            var result = JsonResult<bool>.Default;

            try
            {
                if (!string.IsNullOrWhiteSpace(json))
                {
                    var list = Deserialize<List<MSGNodeInfo>>(json);

                    MSGNodeOperation.Set(list);

                    result.SetResult(true);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("获取微服务节点列表失败", ex);
                result.SetError(new Exception("获取微服务节点列表失败"));
            }
            return Json(result);
        }


        /// <summary>
        /// get api访问计数器
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        [Auth(false)]
        public ActionResult GetApistatisticals(int pageIndex)
        {
            var result = JsonResult<List<Apistatistical>>.Default;
            result.SetResult(ApiCallLogCache.GetApistatisticals(pageIndex));
            return Json(result);
        }
    }
}
