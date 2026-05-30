using ImageViewer.ViewModel.Collections;
using ImageViewer.ViewModel.CustomServiceCollection;
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
    public sealed partial class FileClassificationTiles : UserControl
    {
        FileClassificationTileCollection _vm;

        public FileClassificationTiles()
        {

            InitializeComponent();
            _vm = CustomServiceCollection.GetService<FileClassificationTileCollection>();
            this.DataContext = _vm;
        }
    }
}
