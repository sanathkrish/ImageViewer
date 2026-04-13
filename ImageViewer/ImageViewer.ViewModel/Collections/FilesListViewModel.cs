using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using ImageViewer.Service.File;
using ImageViewer.ViewModel.File;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ImageViewer.ViewModel.Collections
{
    public class FilesListViewModel:BaseViewModel
    {
        public FilesListViewModel(FileService fileService,IMapper mapper)
        {
            _fileServie = fileService;
            _mapper = mapper;
        }
        private FileService _fileServie;
        private IMapper _mapper;

        private ObservableCollection<BaseFileViewModel> _data = new ObservableCollection<BaseFileViewModel>();
        private int _totalCount { get; set; }
        private int _pageSize { get; set; } = 10;
        private int _currentPage { get; set; } = 1;

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
        public ObservableCollection<BaseFileViewModel> Data
        {
            get { return _data; }
            set
            {
                _data = value;
                OnPropertyChanged();
            }
        }

       private string currentPath { get; set; }

        public override async Task InitilizeAsync<String>(String data)
        {
            currentPath = data.ToString();
            await base.InitilizeAsync(data);
            await GetFiles( data.ToString(), CurrentPage, PageSize);

        }

        private async Task GetFiles(string data,int pageNumber,int pageSize)
        {
            var response = await _fileServie.GetFiles(new Model.Pagination.PaginationDataRequest<string> { Filter = data as string, PageNumber = this.CurrentPage, PageSize = this.PageSize });
            this.TotalCount = response.TotalRecords;
            if(this.Data == null)
            {
                this.Data = _mapper.Map<ObservableCollection<BaseFileViewModel>>(response.Data);
            } else
            {
                _mapper.Map<List<BaseFileViewModel>>(response.Data).ForEach(d=> this.Data.Add(d));
                OnPropertyChanged(nameof(Data));
            }
        }

        public async void GetNextAsync() 
        {
            this.CurrentPage++;
           await GetFiles(currentPath, CurrentPage, PageSize);

        }
    }
}
