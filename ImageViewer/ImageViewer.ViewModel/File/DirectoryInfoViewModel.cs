using ImageViewer.Model.File;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageViewer.ViewModel.File
{
    public class DirectoryInfoViewModel: BaseFileViewModel
    {
        private int _fileCount { get; set; }
        private int _directoryCount { get; set; }
        private long _totalSize { get; set; }
        private List<FileInfoViewModel> _files { get; set; } = new List<FileInfoViewModel>();
        private List<DirectoryInfoViewModel> _subDirectories { get; set; } = new List<DirectoryInfoViewModel>();

        public int FileCount
        {
            get { return _fileCount; }
            set
            {
                _fileCount = value;
                OnPropertyChanged();
            }
        }
        public int DirectoryCount
        {
            get { return _directoryCount; }
            set
            {
                _directoryCount = value;
                OnPropertyChanged();
            }
        }
        public long TotalSize
        {
            get { return _totalSize; }
            set
            {
                _totalSize = value;
                OnPropertyChanged();
            }
        }
        public List<FileInfoViewModel> Files
        {
            get { return _files; }
            set
            {
                _files = value;
                OnPropertyChanged();
            }
        }
        public List<DirectoryInfoViewModel> SubDirectories
        {
            get { return _subDirectories; }
            set
            {
                _subDirectories = value;
                OnPropertyChanged();
            }
        }
    }
}
