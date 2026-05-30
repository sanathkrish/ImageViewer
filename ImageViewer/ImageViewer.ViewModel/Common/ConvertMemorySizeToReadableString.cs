using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageViewer.ViewModel.Common
{
    public static class ConvertMemorySizeToReadableString
    {
        public static string Convert(long size)
        {
            if (size < 1024)
                return $"{size} B";
            else if (size < 1024 * 1024)
                return $"{size / 1024.0:F2} KB";
            else if (size < 1024 * 1024 * 1024)
                return $"{size / (1024.0 * 1024):F2} MB";
            else
                return $"{size / (1024.0 * 1024 * 1024):F2} GB";
        }
    }
}
