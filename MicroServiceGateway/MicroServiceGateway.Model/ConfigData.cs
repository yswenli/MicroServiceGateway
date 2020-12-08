using System;
using System.Collections.Generic;
using System.Text;

namespace MicroServiceGateway.Model
{
    public class ConfigData
    {
        public string AppID { get; set; }

        public string Envirment { get; set; }

        public string ID { get; set; }

        public List<ConfigDataItem> Data { get; set; } = new List<ConfigDataItem>();
    }
}
