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
    public sealed partial class ItemView : UserControl
    {
        public ItemView()
        {
            InitializeComponent();
        }

        // Dependency Property
      //  public FileInfoViewModel Vm
      //  {
      //      get => (FileInfoViewModel)GetValue(VmProperty);
      //      set => SetValue(VmProperty, value);
      //  }

      //  public static readonly DependencyProperty VmProperty =
      //      DependencyProperty.Register(
      //          nameof(Vm),
      //          typeof(FileInfoViewModel),
      //          typeof(ItemView),
      //new PropertyMetadata(null, OnVmChanged));

      //  private static void OnVmChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
      //  {
      //      var control = (ItemView)d;
      //      var newValue = e.NewValue;
      //      // Do something when value changes
      //  }
    }
}
