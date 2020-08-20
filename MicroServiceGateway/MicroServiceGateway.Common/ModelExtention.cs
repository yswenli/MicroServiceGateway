/****************************************************************************
*项目名称：MicroServiceGateway.Common
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：MicroServiceGateway.Common
*类 名 称：ModelExtention
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：yswenli@outlook.com
*创建时间：2020/8/20 15:36:35
*描述：
*=====================================================================
*修改时间：2020/8/20 15:36:35
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MicroServiceGateway.Common
{
    /// <summary>
    /// 模型扩展类
    /// </summary>
    public static class ModelExtention
    {
        /// <summary>
        /// 检查空字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="excepts"></param>
        /// <returns></returns>
        public static string CheckNull<T>(this T t, params string[] excepts) where T : class, new()
        {
            if (t == null) return "传入参数不能为空！";

            var type = typeof(T);

            var properties = type.GetProperties();

            if (properties == null) return "传入的参数不包含任何属性！";

            if (excepts != null && excepts.Any())
            {
                StringBuilder sb = new StringBuilder();

                foreach (var item in properties)
                {
                    if (excepts.Contains(item.Name))
                    {
                        break;
                    }

                    var val = item.GetValue(t, null);

                    if (item.PropertyType.IsPrimitive)
                    {
                        if (item.PropertyType == typeof(byte)
                            || item.PropertyType == typeof(short)
                            || item.PropertyType == typeof(int)
                            || item.PropertyType == typeof(long)
                            || item.PropertyType == typeof(float)
                            || item.PropertyType == typeof(double)
                            || item.PropertyType == typeof(decimal))
                        {
                            if (decimal.TryParse(val.ToString(), out decimal v))
                            {
                                if (v == 0)
                                {
                                    sb.Append($"{item.Name}值不能为空！ ");
                                }
                            }
                        }
                        if (item.PropertyType == typeof(DateTime))
                        {
                            var v = Convert.ToDateTime(val);

                            if (v.Year == 1)
                            {
                                sb.Append($"{item.Name}值不能为空！ ");
                            }
                        }
                    }
                    else if (item.PropertyType == typeof(string))
                    {
                        if (string.IsNullOrEmpty(val.ToString()))
                        {
                            sb.Append($"{item.Name}值不能为空！ ");
                        }
                    }
                    else if (item.PropertyType.IsEnum)
                    {
                        break;
                    }
                    else
                    {
                        break;
                    }
                }
                return sb.ToString();
            }
            else
            {
                StringBuilder sb = new StringBuilder();

                foreach (var item in properties)
                {
                    var val = item.GetValue(t, null);

                    if (val == null)
                    {
                        sb.Append($"{item.Name}值不能为空！ ");
                        break;
                    }

                    if (item.PropertyType.IsPrimitive)
                    {
                        if (item.PropertyType == typeof(byte)
                            || item.PropertyType == typeof(short)
                            || item.PropertyType == typeof(int)
                            || item.PropertyType == typeof(long)
                            || item.PropertyType == typeof(float)
                            || item.PropertyType == typeof(double)
                            || item.PropertyType == typeof(decimal))
                        {

                            if (decimal.TryParse(val.ToString(), out decimal v))
                            {
                                if (v == 0)
                                {
                                    sb.Append($"{item.Name}值不能为空！ ");
                                }
                            }
                        }
                        if (item.PropertyType == typeof(DateTime))
                        {
                            var v = Convert.ToDateTime(val);

                            if (v.Year == 1)
                            {
                                sb.Append($"{item.Name}值不能为空！ ");
                            }
                        }
                    }
                    else if (item.PropertyType == typeof(string))
                    {
                        if (string.IsNullOrEmpty(val.ToString()))
                        {
                            sb.Append($"{item.Name}值不能为空！ ");
                        }
                    }
                    else if (item.PropertyType.IsEnum)
                    {
                        break;
                    }
                    else
                    {
                        break;
                    }
                }
                return sb.ToString();
            }
        }

    }
}
