/****************************************************************************
*项目名称：MicroServiceGateway.Model
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：MicroServiceGateway.Model
*类 名 称：Performace
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：yswenli@outlook.com
*创建时间：2020/8/19 15:49:57
*描述：
*=====================================================================
*修改时间：2020/8/19 15:49:57
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/

namespace MicroServiceGateway.Model
{
    /// <summary>
    /// 性能统计项
    /// </summary>
    public class Performace
    {
        /// <summary>
        /// CPU
        /// </summary>
        public float CPU { get; set; }

        /// <summary>
        /// MemoryUsage
        /// </summary>
        public float MemoryUsage { get; set; }
        /// <summary>
        /// TotalThreads
        /// </summary>
        public float TotalThreads { get; set; }
        /// <summary>
        /// HandleCount
        /// </summary>
        public float HandleCount { get; set; }
        /// <summary>
        /// BytesRec
        /// </summary>
        public float BytesRec { get; set; }
        /// <summary>
        /// BytesSen
        /// </summary>
        public float BytesSen { get; set; }
    }
}
