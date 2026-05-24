using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageViewer.Model.File
{
    public class DuplicateFileInfo
    {
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public long FileSize { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string Hash { get; set; } = string.Empty;
        public bool IsDuplicate { get; set; }
        public string DuplicateOf { get; set; }
        public string Extension { get; set; }
    }
}
