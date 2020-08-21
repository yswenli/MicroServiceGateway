/****************************************************************************
*项目名称：MicroServiceGateway.Manager.Controllers
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：MicroServiceGateway.Manager.Controllers
*类 名 称：VerificationController
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：yswenli@outlook.com
*创建时间：2020/8/21 10:56:24
*描述：
*=====================================================================
*修改时间：2020/8/21 10:56:24
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using MicroServiceGateway.Manager.Libs.Verification;
using MicroServiceGateway.Model;
using SAEA.MVC;
using System;
using System.IO;

namespace MicroServiceGateway.Manager.Controllers
{
    public class VerificationController : Controller
    {
        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Index(int id = 0)
        {
            try
            {
                HttpContext.Current.Response.ContentType = "image/Gif";

                using (MemoryStream m = new MemoryStream())
                {
                    VerificationCode va = new VerificationCode(105, 30, 4, id);
                    var s = va.Create(m);
                    string code = va.IdentifyingCode;
                    HttpContext.Current.Session["code"] = code;
                    HttpContext.Current.Response.BinaryWrite(m.ToArray());
                    return new EmptyResult();
                }
            }
            catch (Exception ex)
            {
                return Json(new JsonResult<bool>() { Code = 999, Data = false, Message = ex.Message });
            }
        }

        /// <summary>
        /// 验证验证码
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public ActionResult Check(string code)
        {
            try
            {
                if (!string.IsNullOrEmpty(code))
                {
                    code = code.ToLower();

                    var rcode = HttpContext.Current.Session["code"];

                    if (rcode != null && rcode.ToString().ToLower() == code)
                    {
                        return Json(new JsonResult<bool>() { Code = 1, Data = true });
                    }
                }

                return Json(new JsonResult<bool>() { Code = 1, Data = false, Message = "验证码不正确！" });
            }
            catch (Exception ex)
            {
                return Json(new JsonResult<bool>() { Code = 1, Data = false, Message = "验证码不正确！" });
            }
        }
    }
}
