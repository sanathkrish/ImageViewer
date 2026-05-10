using CommunityToolkit.Mvvm.ComponentModel;
using ImageViewer.Service.File;
using ImageViewer.ViewModel.Common;
using ImageViewer.ViewModel.File;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageViewer.ViewModel.Views
{
    public class BaseItemViewModel:BaseFileViewModel
    {
        ThumbnailService _thumbnailService;
        public Action<byte[]> onThumbnailAvailableCallBack;
        public BaseItemViewModel()
        {
            _thumbnailService = CustomServiceCollection.CustomServiceCollection.ServiceProvider.GetRequiredService<ThumbnailService>();
        }
        private bool _isImage;
       
        public bool IsImage
        {
            get { return _isImage; }
            set
            {
                _isImage = value;
                OnPropertyChanged();
            }
        }
        private long _size;
        public long Size
        {
            get { return _size; }
            set
            {
                _size = value;
                OnPropertyChanged();
            }
        }

        public Visibility ItIsDirectory_v
        {
            get { return IsDirectory ? Visibility.Visible : Visibility.Collapsed; }
        }

        public Visibility ItIsFile_v
        {
            get { return IsFile ? Visibility.Visible : Visibility.Collapsed; }
            
        }

        private string _itemImage => FileType.ToString() switch
        {
            "File" => GetThumbnailPath(Path),
            "Directory"   => "/ImageViewer.Controls/Resources/Folder_Img.png",
            _ => "file_image.png"
        };
        public string ItemImage
        {     get { return _itemImage; }
           
        }

        public string SizeText
        {
            get
            {
                if (Size >= 1073741824)
                    return $"{Size / 1073741824.0:F2} GB";
                else if (Size >= 1048576)
                    return $"{Size / 1048576.0:F2} MB";
                else if (Size >= 1024)
                    return $"{Size / 1024.0:F2} KB";
                else
                    return $"{Size} B";
            }
        }

        public void RetriggerProperty()
        {
            OnPropertyChanged("ItemImage");
            OnPropertyChanged(nameof(IsDirectory));
            OnPropertyChanged(nameof(IsFile));
        }

        private string GetThumbnailPath(string filePath)
        {
            if (ValidateFileIsImage.IsFileImage(filePath))
            {

            _thumbnailService.GetThumbnail(filePath, (data) =>
            {
                onThumbnailAvailableCallBack?.Invoke(data);
            });
            }

            return "/ImageViewer.Controls/Resources/file_image.png";
        }

    }
}
