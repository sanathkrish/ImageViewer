using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ImageViewer.Service.File;
using ImageViewer.Service.Interfaces;
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
        public FilesListViewModel(FileService fileService,IMapper mapper,INavigationService navigationService)
        {
            _fileServie = fileService;
            _mapper = mapper;
            _navigationService = navigationService;
            this._selectedItemCommand = new RelayCommand<BaseFileViewModel>((param) =>
            {
                if(param != null && param.IsDirectory)
                {
                    this._navigationService.Navigate("explorer_content", "ExplorerContent", param.Path);
                }
            });
            NavigateBack = new Views.NavigateBackItemViewModel(navigationService, "explorer_content");
        }
        private FileService _fileServie;
        private IMapper _mapper;
        private INavigationService _navigationService;
        private Views.NavigateBackItemViewModel _navigateBack;
        public Views.NavigateBackItemViewModel NavigateBack {  get { return _navigateBack; } set { _navigateBack = value; OnPropertyChanged(); }  }

        private ObservableCollection<BaseFileViewModel> _data = new ObservableCollection<BaseFileViewModel>();
        private int _totalCount { get; set; }
        private int _pageSize { get; set; } = 100;
        private int _currentPage { get; set; } = 1;

        private RelayCommand<BaseFileViewModel> _selectedItemCommand;
       public RelayCommand<BaseFileViewModel> SelectItemCommand { get { return _selectedItemCommand; } }

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

        public async Task GetNextAsync() 
        {
            this.CurrentPage++;
           await GetFiles(currentPath, CurrentPage, PageSize);

        }
    }
}
