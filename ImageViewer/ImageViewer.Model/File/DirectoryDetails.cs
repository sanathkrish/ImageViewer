using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageViewer.Model.File
{
    public class DirectoryDetails:BaseFile
    {
        public int FileCount { get; set; }
        public int DirectoryCount { get; set; }
        public long TotalSize { get; set; }
        public List<FileInfo> Files { get; set; } = new List<FileInfo>();
        public List<DirectoryDetails> SubDirectories { get; set; } = new List<DirectoryDetails>();
    }
}
