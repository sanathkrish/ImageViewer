using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageViewer.Model.Data
{
    public class DriverInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string Type { get; set;  }
        public long TotalSize { get; set; }
        public long FreeSpace { get; set;  }
        public DateTime? DateAdded { get; set; }
    }
}
