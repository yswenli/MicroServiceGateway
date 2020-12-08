using System;
using System.Collections.Generic;
using System.Text;

namespace MicroServiceGateway.Model
{
    public class ConfigDataItem
    {
        public string ID { get; set; }
        
        public string ConfigDataID { get; set; }

        public int OperationMode { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }

        public string Created { get; set; }

        public string Updated { get; set; }

        public bool IsPublished { get; set; }

        public string Published { get; set; }
    }
}
