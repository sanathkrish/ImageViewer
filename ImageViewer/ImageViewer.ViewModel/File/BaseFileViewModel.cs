using ImageViewer.Model;
using ImageViewer.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ImageViewer.ViewModel.File
{
    public class BaseFileViewModel : BaseViewModel
    {
        private string _name { get; set; }
        private string _path { get; set; }
        private FileType _fileType { get; set; }
        private DateTime? _created { get; set; }
        private DateTime? _lastModified { get; set; }


        public string Name
        {
                       get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }
        public string Path
        {
            get { return _path; }
            set
            {
                _path = value;
                OnPropertyChanged();
            }
        }
        public FileType FileType
        {
            get { return _fileType; }
            set
            {
                _fileType = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsDirectory));
                OnPropertyChanged(nameof(IsFile));
            }
        }
        public DateTime? Created
        {
            get { return _created; }
            set
            {
                _created = value;
                OnPropertyChanged();
            }
        }
        public DateTime? LastModified
        {
            get { return _lastModified; }
            set
            {
                _lastModified = value;
                OnPropertyChanged();
            }
        }

        public bool IsDirectory => FileType == FileType.Directory;
        public bool IsFile => FileType == FileType.File;

        public override void Initilize<BaseFile>(BaseFile data)
        {
            base.Initilize(data);
        }
    }
}
