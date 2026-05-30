using CommunityToolkit.Mvvm.ComponentModel;
using ImageViewer.ViewModel.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageViewer.ViewModel.Views
{
    public class FileClassificationTileViewModel:BaseViewModel
    {
        private string _name;
        private string _description;
        private int _count;
        private long _size;

        public string Name { get {return _name; } set { _name = value; } }
        public int Count { get {return _count; } set { _count = value; } }
        public long Size { get {return _size; } set { _size = value; } }
        public string SizeString { get=>ConvertMemorySizeToReadableString.Convert(_size); }
    }
}
