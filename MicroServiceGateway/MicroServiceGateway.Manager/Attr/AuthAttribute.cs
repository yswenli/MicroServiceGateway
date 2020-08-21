﻿/****************************************************************************
*项目名称：MicroServiceGateway.Manager.Attr
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：MicroServiceGateway.Manager.Attr
*类 名 称：AuthAttribute
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：yswenli@outlook.com
*创建时间：2020/8/21 10:45:15
*描述：
*=====================================================================
*修改时间：2020/8/21 10:45:15
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using MicroServiceGateway.Manager.Libs;
using MicroServiceGateway.Model;
using SAEA.Common;
using SAEA.MVC;
using System.Diagnostics;

namespace MicroServiceGateway.Manager.Attr
{
    /// <summary>
    /// 验证并记录日志
    /// </summary>
    public class AuthAttribute : ActionFilterAttribute
    {
        Stopwatch _stopwatch;

        bool _isAdmin = false;

        public AuthAttribute(bool isEnabled)
        {

        }

        public AuthAttribute(bool isAdmin, bool isEnabled) : this(isEnabled)
        {
            _isAdmin = isAdmin;
        }


        public override bool OnActionExecuting()
        {
            _stopwatch = Stopwatch.StartNew();

            if (!HttpContext.Current.Session.Keys.Contains("uid"))
            {
                HttpContext.Current.Response.SetCached(new JsonResult(new JsonResult<string>() { Code = 3, Message = "当前操作需要登录！" }));

                HttpContext.Current.Response.End();

                return false;
            }
            if (_isAdmin)
            {
                var user = UserHelper.Get(HttpContext.Current.Session["uid"].ToString());

                if (user.Role != Role.Admin)
                {
                    HttpContext.Current.Response.SetCached(new JsonResult(new JsonResult<string>() { Code = 4, Message = "当前操作权限不足，请联系管理员！" }));

                    HttpContext.Current.Response.End();

                    return false;
                }
            }

            return true;
        }

        public override void OnActionExecuted(ActionResult result)
        {
            _stopwatch.Stop();

            LogHelper.Info(HttpContext.Current.Request.Url, HttpContext.Current.Request.Parmas, result, _stopwatch.ElapsedMilliseconds);
        }
    }
}
