using CommunityToolkit.Mvvm.Input;
using ImageViewer.Service.Interfaces;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageViewer.ViewModel.Views
{
    public class NavigateBackItemViewModel:BaseViewModel
    {
        private INavigationService _navigationService;
        public string frameName { get; set; }
        public Frame NavigationSection { get; set; }
        private bool _canDisableNavigation;
        public bool CanDisableNavigation
        {
            get { return _canDisableNavigation; }
            set { _canDisableNavigation = value;OnPropertyChanged(); }
        }
        public NavigateBackItemViewModel(INavigationService navigationService,string frame)
        {
            _navigationService = navigationService;
            NavigationSection = navigationService.GetNavigationFrame(frame);
            frameName = frame;
            CanDisableNavigation = NavigationSection.CanGoBack;
            this.NavigationBackCommand = new RelayCommand(() =>
            {
                _navigationService.NavigateBack(frameName);
            });
        }
    }
}
