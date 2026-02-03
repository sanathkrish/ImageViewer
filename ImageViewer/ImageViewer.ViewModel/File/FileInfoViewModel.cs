using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageViewer.ViewModel.File
{
    public class FileInfoViewModel: BaseFileViewModel
    {
        private string _extension { get; set; }
        private long _size { get; set; }

        public string Extension
        {
            get { return _extension; }
            set
            {
                _extension = value;
                OnPropertyChanged();
            }
        }

        public long Size
        {
            get { return _size; }
            set
            {
                _size = value;
                OnPropertyChanged();
            }
        }
    }
}
