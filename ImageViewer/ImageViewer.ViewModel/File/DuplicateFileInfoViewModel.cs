using ImageViewer.Model.File;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageViewer.ViewModel.File
{
    public class DuplicateFileInfoViewModel:BaseViewModel
    {
        public DuplicateFileInfo _metaData;
        public DuplicateFileInfoViewModel(DuplicateFileInfo metaData)
        {
            _metaData = metaData;
        }
    }
}
