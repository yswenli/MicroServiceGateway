using System.Collections;
using System.Collections.Generic;

namespace MicroServiceGateway.Model
{
    /// <summary>
    /// 调用次数统计
    /// </summary>
    public class Apistatistical
    {
        public string Url { get; set; }

        public long Count { get; set; }
    }
}
