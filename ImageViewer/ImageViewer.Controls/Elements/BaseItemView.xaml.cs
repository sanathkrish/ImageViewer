using ImageViewer.ViewModel.File;
using ImageViewer.ViewModel.Views;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ImageViewer.Controls.Elements
{
    public sealed partial class BaseItemView : UserControl,IDisposable
    {
        protected BaseItemViewModel _vm { get; set; }
        public BaseItemView()
        {
            InitializeComponent();
        }

        public void Initilize(BaseItemViewModel vm)
        {
            _vm = vm;
            DataContext = _vm;
            if (DataContext != null) {
            this._vm.onThumbnailAvailableCallBack += (byte[] thumbnailData) =>
            {
                this.DispatcherQueue.TryEnqueue(() =>
                {
                    using var stream = new InMemoryRandomAccessStream();
                    stream.WriteAsync(thumbnailData.AsBuffer()).GetAwaiter().GetResult();
                    stream.Seek(0);

                    var bitmap = new BitmapImage();
                    bitmap.SetSource(stream);
                    this.ItemImage.Source = bitmap;
                });
            };
            }
        }

        public void Dispose()
        {
            if(this._vm != null)
            {
                this._vm.isViewUnassigned = true;
            }
        }
    }
}
