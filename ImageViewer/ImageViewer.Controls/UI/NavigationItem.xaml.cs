using ImageViewer.ViewModel.Views;
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

namespace ImageViewer.Controls.UI
{
    public sealed partial class NavigationItem : UserControl
    {
        public NavigationItemViewModel _vm;
        public NavigationItemViewModel vm
        {
            get
            {
                return _vm  ;
            }
            set { _vm = value; }
        }
        //public static readonly DependencyProperty VmProperty = DependencyProperty.Register("Vm", typeof(NavigationItemViewModel), typeof(NavigationItem), new PropertyMetadata(null));
        public NavigationItem()
        {
            InitializeComponent();
            DataContextChanged += NavigationItem_DataContextChanged;

        }

        private void NavigationItem_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            vm = args.NewValue as NavigationItemViewModel;
        }
    }
}
