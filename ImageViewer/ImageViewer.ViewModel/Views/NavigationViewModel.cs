using CommunityToolkit.Mvvm.Input;
using ImageViewer.Service.Interfaces;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageViewer.ViewModel.Views
{
    public class NavigationViewModel:BaseViewModel
    {
        private INavigationService _navigationService;
        private ObservableCollection<NavigationItemViewModel> _items = new ObservableCollection<NavigationItemViewModel>();
        public ObservableCollection<NavigationItemViewModel> Items {  get { return _items; } }
        public NavigationViewModel(INavigationService navigationService) {
        _navigationService = navigationService;
        }

        public override Task InitilizeAsync<String>(String data)
        {
            Items.Add(new NavigationItemViewModel() 
            {
                Name = "All Files",
                UseStandard = true,
                NavigationCommand = new RelayCommand(async () => await Navigate("main_window", "HomePage", null)) });
            Items.Add(new NavigationItemViewModel()
            {
                Name = "Duplicate Files",
                UseStandard = true,
                NavigationCommand = new RelayCommand(async () => await Navigate("main_window", "Explorer", null))
            });
            Items.Add(new NavigationItemViewModel()
            {
                Name = "Duplicate Images",
                UseStandard = true,
                NavigationCommand = new RelayCommand(async () => await Navigate("main_window", "ImageAnalysis", null))
            });
            Items.Add(new NavigationItemViewModel()
            {
                Name = "Similar Images",
                UseStandard = true,
                NavigationCommand = new RelayCommand(async () => await Navigate("main_window", "HomePage", null))
            });
            Items.Add(new NavigationItemViewModel()
            {
                Name = "Large Files",
                UseStandard = true,
                NavigationCommand = new RelayCommand(async () => await Navigate("main_window", "HomePage", null))
            });
            Items.Add(new NavigationItemViewModel()
            {
                Name = "Empty Files",
                UseStandard = true,
                NavigationCommand = new RelayCommand(async () => await Navigate("main_window", "HomePage", null))
            });
            return base.InitilizeAsync(data);
        }
        private async Task Navigate(string frame,string navigation,object parameters)
        {
            _navigationService.Navigate("main_window", navigation, null);
        }
        public void RegisterFrame(string frameName,Frame frameType)
        {
            _navigationService.RegisterFrame("main_window", frameType);
        }

        public void RegisterNavigation(string frame, Type content)
        {
            _navigationService.RegisterNavigation("main_window", content);
        }

    }
}
