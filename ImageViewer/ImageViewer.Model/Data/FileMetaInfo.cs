using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageViewer.Model.Data
{
    public class FileMetaInfo
    {
        public int Id { get; set;  } = -1;
        public string FileType { get; set; }
        public int FileId { get; set; }
        public int? Duplicate { get; set; }
        public bool IsBlurred { get; set; }
        public bool IsCorrupted { get; set; }
        public int? Similar { get; set; }
        public bool IsEmpty { get; set; }
        public string AdditionalMetaInfo { get; set; }
    }

}
