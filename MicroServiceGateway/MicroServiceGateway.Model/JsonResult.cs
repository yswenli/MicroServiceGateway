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
}
