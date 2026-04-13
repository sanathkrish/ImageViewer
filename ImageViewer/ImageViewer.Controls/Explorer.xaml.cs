using ImageViewer.ViewModel.Collections;
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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ImageViewer.Controls
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Explorer : Page
    {
        private FilesListViewModel _vm;
        public Explorer()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e); 
            _vm = CustomServiceCollection.ServiceProvider.GetService<FilesListViewModel>();
            _vm.InitilizeAsync("F:\\").ConfigureAwait(false);
            this.DataContext = _vm;
        }
        private void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {

        }
    }
}
