using ImageViewer.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageViewer.Model
{
    public abstract class BaseFile
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public FileType FileType { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? LastModified { get; set; }
    }
}
