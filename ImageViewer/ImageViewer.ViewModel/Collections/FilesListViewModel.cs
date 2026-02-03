using ImageViewer.Service.File;
using ImageViewer.ViewModel.File;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageViewer.ViewModel.Collections
{
    public class FilesListViewModel:BaseViewModel
    {
        public FilesListViewModel(FileService fileService)
        {
            _fileServie = fileService;
        }
        private FileService _fileServie;

        private List<BaseFileViewModel> _data = new List<BaseFileViewModel>();
        private int _totalCount { get; set; }
        private int _pageSize { get; set; }
        private int _currentPage { get; set; } = 0;

        public int CurrentPage
        {
            get { return _currentPage; }
            set
            {
                _currentPage = value;
                OnPropertyChanged();
            }
        }
        public int PageSize
        {
            get { return _pageSize; }
            set
            {
                _pageSize = value;
                OnPropertyChanged();
            }
        }
        public int TotalCount
        {
            get { return _totalCount; }
            set
            {
                _totalCount = value;
                OnPropertyChanged();
            }
        }
        public List<BaseFileViewModel> Data
        {
            get { return _data; }
            set
            {
                _data = value;
                OnPropertyChanged();
            }
        }

        public override async Task InitilizeAsync<String>(String data)
        {
            await base.InitilizeAsync(data);
            var response = await _fileServie.GetFiles(new Model.Pagination.PaginationDataRequest<string> { Filter = data as string,PageNumber = this.CurrentPage,PageSize = this.PageSize});
        }
    }
}
