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
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ImageViewer.Controls.Elements;

public sealed partial class ExplorerContent : Page
{
    public FilesListViewModel _vm;
    public ExplorerContent()
    {
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        _vm = CustomServiceCollection.ServiceProvider.GetService<FilesListViewModel>();
        var param = e.Parameter as string;
        _vm.InitilizeAsync(param).ConfigureAwait(false);
        this.DataContext = _vm;
    }

    private void GridView_ItemClick(object sender, ItemClickEventArgs e)
    {
        if(this._vm.SelectItemCommand != null)
        {
            this._vm.SelectItemCommand.Execute(e.ClickedItem as object);
        }
    }


    private async void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
    {
        var sv = (ScrollViewer)sender;
        double distanceFromBottom = sv.ExtentHeight - sv.VerticalOffset - sv.ViewportHeight;

        if (distanceFromBottom < 100) // threshold in pixels
        {
           await _vm.GetNextAsync();
        }
    }
}
