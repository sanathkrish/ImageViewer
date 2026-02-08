using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageViewer.Model
{
    public class ThumbnailInfo
    {
        public string Path { get; set; }
        //public byte[] ThumbnailData { get; set; }
        public string UniqueId { get; set; }
        public byte[] Data { get; set; }
        public Action<byte[]> ImageCallback { get; set; }
    }
}
