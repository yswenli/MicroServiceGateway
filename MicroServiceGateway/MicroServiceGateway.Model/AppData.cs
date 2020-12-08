using System;
using System.Collections.Generic;
using System.Text;

namespace MicroServiceGateway.Model
{
    /// <summary>
    /// 分布式配置
    /// </summary>
    public class AppData
    {
        public string ID { get; set; }

        public string Name { get; set; }

        public List<string> Envirments { get; set; } = new List<string>();        
        public string Created { get; set; }

        public string Updated { get; set; }
    }
}
