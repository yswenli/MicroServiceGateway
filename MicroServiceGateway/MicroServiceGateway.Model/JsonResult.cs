/****************************************************************************
*项目名称：MicroServiceGateway.Model
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：MicroServiceGateway.Model
*类 名 称：JsonResult
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：yswenli@outlook.com
*创建时间：2020/8/21 10:47:15
*描述：
*=====================================================================
*修改时间：2020/8/21 10:47:15
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/

using System;

namespace MicroServiceGateway.Model
{
    public class JsonResult<T>
    {
        public int Code
        {
            get; set;
        }

        public string Message
        {
            get; set;
        }

        public T Data
        {
            get; set;
        }


    }

    public static class JsonResultExtention
    {
        public static JsonResult<T> SetResult<T>(this JsonResult<T> result, string msg = "操作完成", T t = default(T), int code = 0)
        {
            result.Code = code;
            result.Message = msg;
            result.Data = t;
            return result;
        }


        public static JsonResult<T> SetError<T>(this JsonResult<T> result, Exception ex, int code = 1)
        {
            result.Code = code;
            result.Message = $"系统异常：{ex.Message}";
            result.Data = default(T);
            return result;
        }
    }
}
