using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using ImageViewer.ViewModel.Views;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ImageViewer.Controls.Elements;

public sealed partial class NavigateBackItemView : UserControl
{
    private NavigateBackItemViewModel _vm;

    public NavigateBackItemView()
    {
        InitializeComponent();
        DataContextChanged += onDataContextChange;
    }

    private void onDataContextChange(FrameworkElement sender, DataContextChangedEventArgs args)
    {
       if(args != null && args.NewValue != null && args.NewValue is NavigateBackItemViewModel)
        {
            _vm = args.NewValue as NavigateBackItemViewModel;
        }
    }
}
