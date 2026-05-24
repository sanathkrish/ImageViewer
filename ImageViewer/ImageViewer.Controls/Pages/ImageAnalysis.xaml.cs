using ImageViewer.Controls.Elements;
using ImageViewer.Service.Interfaces;
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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ImageViewer.Controls.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ImageAnalysis : Page
    {
        public ImageAnalysis()
        {
            InitializeComponent();
            var navigationService = ViewModel.CustomServiceCollection.CustomServiceCollection.ServiceProvider.GetService<INavigationService>();
            navigationService.RegisterFrame("data_content", data_content);
            navigationService.RegisterFrame("details_content", details_content);
            navigationService.RegisterNavigation("data_content", typeof(ExtensionContent));
            navigationService.RegisterNavigation("details_content", typeof(ExtensionInfoDetails));
            NavigateToPath("F:\\");
        }

        private void NavigateToPath(string path)
        {
            data_content.Navigate(typeof(ExtensionContent), path);
        }
    }
}
