using ImageViewer.Controls.Pages;
using ImageViewer.Controls.UI;
using ImageViewer.Service.Interfaces;
using ImageViewer.ViewModel.CustomServiceCollection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ApplicationSettings;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ImageViewer.Controls
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class InitialWindow : Window
    {
        private INavigationService _navigationService;
        public InitialWindow()
        {
            InitializeComponent();
            _navigationService = CustomServiceCollection.GetService<INavigationService>();
            _navigationService.RegisterFrame("main_window", main_window);
            _navigationService.RegisterNavigation("main_window", typeof(MainLandingPage));
            _navigationService.Navigate("main_window", "MainLandingPage", null);
        }


        
    }
}
