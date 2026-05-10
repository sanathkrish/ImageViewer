using CommunityToolkit.Mvvm.Input;
using ImageViewer.Controls.Elements;
using ImageViewer.Service.Interfaces;
using ImageViewer.ViewModel.Collections;
using ImageViewer.ViewModel.CustomServiceCollection;
using ImageViewer.ViewModel.File;
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
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Explorer : Page
    {
        public Explorer()
        {
            InitializeComponent();
            var navigationService = ViewModel.CustomServiceCollection.CustomServiceCollection.ServiceProvider.GetService<INavigationService>();
            navigationService.RegisterFrame("explorer_content", explorer_content);
            navigationService.RegisterNavigation("explorer_content", typeof(ExplorerContent));
            NavigateToPath("F:\\");
        }

        private void NavigateToPath(string path)
        {
            //explorer_content.Navigated += (s, e) =>
            //    {
            //        var content = e.Content as ExplorerContent;
            //        content._vm.SelectItemCommand = new RelayCommand<BaseFileViewModel>(item =>
            //        {
            //            if (item.IsDirectory)
            //            {
            //                NavigateToPath(item.Path);
            //            }
            //        });
            //    };
            explorer_content.Navigate(typeof(ExplorerContent), path);

        }


        //private void GridView_ItemClick(object sender, ItemClickEventArgs e)
        //{
        //    var item = e.ClickedItem as BaseFileViewModel;
        //    if (item != null) { 
        //       if(item.IsDirectory)
        //        {
        //            // _vm.InitilizeAsync(item.Path).ConfigureAwait(false);
        //            explorer_content.Navigate(typeof(ExplorerContent),item.Path);
        //            explorer_content.Navigated += (s, e) => 
        //            {
        //            };
        //        }
        //    }
        //}
    }
}
