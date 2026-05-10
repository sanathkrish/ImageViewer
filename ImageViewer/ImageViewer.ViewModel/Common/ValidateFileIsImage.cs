using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageViewer.ViewModel.Common
{
    public static class ValidateFileIsImage
    {
        private static List<string> imageExtenstions = new List<string> { ".jpg", ".jpeg", ".png", ".bmp", ".gif", ".webp" };
        private static List<string> _videoExtenstions = new List<string> { ".mp4", ".avi", ".mkv", ".mov", ".wmv", ".flv" };
        public static bool IsFileImage(string filePath)
        {
            string ext = System.IO.Path.GetExtension(filePath).ToLower();

            if (imageExtenstions.Contains(ext))
                return true;
            return false;

        }
    }
}
