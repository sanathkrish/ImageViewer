using ImageViewer.Model;
using ImageViewer.Service.BackgroundWorkers;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageViewer.Service.File
{
    public class ThumbnailService
    {
        private ThumbnailBackgroundWorker _thumbnailBackgroundWorker;
        public ThumbnailService(ThumbnailBackgroundWorker worker)
        {
            _thumbnailBackgroundWorker = worker;
             _thumbnailBackgroundWorker.Initialize();
             _thumbnailBackgroundWorker.Run(default);
        }


        public void GetThumbnail(string filePath, Action<byte[]> callBack)
        {

            var thumbnailInfo = new ThumbnailInfo
            {
                Path = filePath,
                ImageCallback = callBack
            };
            _thumbnailBackgroundWorker.AddToQueue(thumbnailInfo);
            _thumbnailBackgroundWorker.Initialize();
            _thumbnailBackgroundWorker.Run(null);
        }
    }
}
