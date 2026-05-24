using ImageViewer.Service.File;
using ImageViewer.ViewModel.File;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageViewer.ViewModel.Collections
{
    public class DuplicateFileCollection:BaseViewModel
    {
        public DuplicateFileCollection()
        {
            this.duplicateImageService = CustomServiceCollection.CustomServiceCollection.GetService<DuplicateImageService>();
            _=this.duplicateImageService.ScanDuplicateFiles("F:\\");
        }
        private DuplicateImageService duplicateImageService;
        private ObservableCollection<DuplicateFileInfoViewModel> _duplicateFiles = new ObservableCollection<DuplicateFileInfoViewModel>();
        public ObservableCollection<DuplicateFileInfoViewModel> DuplicateFiles
        {
            get { return _duplicateFiles; }
            set
            {
                _duplicateFiles = value;
                OnPropertyChanged();
            }
        }
    }
}
