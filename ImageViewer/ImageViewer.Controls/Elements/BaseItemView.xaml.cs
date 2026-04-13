using ImageViewer.ViewModel.File;
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

namespace ImageViewer.Controls.Elements
{
    public sealed partial class BaseItemView : UserControl
    {
        protected BaseFileViewModel vm { get; set; }
        protected bool IsImage { get; set; } = false;
        public BaseItemView()
        {
            InitializeComponent();
            this.DataContextChanged += BaseItemView_DataContextChanged;
        }

        private void BaseItemView_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            this.vm = args.NewValue as BaseFileViewModel;
        }
    }
}
