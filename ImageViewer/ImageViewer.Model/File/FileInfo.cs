using ImageViewer.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageViewer.Model.File
{
    public class FileInfo : BaseFile
    {
        public string Extension { get; set; }
        public long Size { get; set; }
    }
}
