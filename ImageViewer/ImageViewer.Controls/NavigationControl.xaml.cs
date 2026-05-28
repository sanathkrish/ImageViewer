using ImageViewer.Controls.Pages;
using ImageViewer.Service.Interfaces;
using ImageViewer.ViewModel.CustomServiceCollection;
using ImageViewer.ViewModel.Views;
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

namespace ImageViewer.Controls
{
    public sealed partial class NavigationControl : UserControl
    {
        private NavigationViewModel _vm;
        public NavigationControl()
        {
            InitializeComponent();
            _vm = CustomServiceCollection.ServiceProvider.GetService<NavigationViewModel>();
            if (_vm != null)
            {
                //_vm.RegisterFrame("main_window", ContentFrame);
                _vm.RegisterNavigation("main_window", typeof(Explorer));
                _vm.RegisterNavigation("main_window", typeof(ImageAnalysis));
            }
        }

        private void NavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            //var selectedItem = args.SelectedItem as NavigationViewItem;
            //string tag = selectedItem.Tag.ToString().ToLower();

            //switch (tag)
            //{
            //    case "explorer":
            //        ContentFrame.Navigate(typeof(Explorer));
            //        break;

            //    case "imageanalysis":
            //        ContentFrame.Navigate(typeof(ImageAnalysis));
            //        break;
            //    case "duplicatefiles":
            //        ContentFrame.Navigate(typeof(DuplicateFile));
            //        break;

            //}
        }
    }
}
