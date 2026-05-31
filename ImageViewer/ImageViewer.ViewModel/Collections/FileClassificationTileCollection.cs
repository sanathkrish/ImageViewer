using ImageViewer.ViewModel.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageViewer.ViewModel.Collections
{
    public class FileClassificationTileCollection:BaseViewModel
    {
        private List<Views.FileClassificationTileViewModel> _items;
        public List<Views.FileClassificationTileViewModel> Items { get { return _items; } set { _items = value; } }
        private EventAggreator _eventAggreator;
        public FileClassificationTileCollection(EventAggreator eventAggreator)
        {
            _eventAggreator = eventAggreator;
            this.InitilizeAsync<string>(string.Empty).ConfigureAwait(false);
        }

        public override Task InitilizeAsync<String>(String data)
        {
           
            var allFiles = new Views.FileClassificationTileViewModel { Name = "All Files" };
            _eventAggreator.Subscribe<Tuple<long, long>>(Common.EventAggregatorConstants.AllFilesCount, allFiles.UpdateCount);
            var imageCount = new Views.FileClassificationTileViewModel { Name = "Images" };
            _eventAggreator.Subscribe<Tuple<long, long>>(Common.EventAggregatorConstants.ImageCount, imageCount.UpdateCount);
            var documentCount = new Views.FileClassificationTileViewModel { Name = "Documents" };
            _eventAggreator.Subscribe<Tuple<long, long>>(Common.EventAggregatorConstants.VideoCount, documentCount.UpdateCount);
            var videoCount = new Views.FileClassificationTileViewModel { Name = "Videos" };
            _eventAggreator.Subscribe<Tuple<long, long>>(Common.EventAggregatorConstants.DuplicateCount, videoCount.UpdateCount);
            var duplicateCount = new Views.FileClassificationTileViewModel { Name = "Duplicate" };
            _eventAggreator.Subscribe<Tuple<long, long>>(Common.EventAggregatorConstants.SimilarImageCount, duplicateCount.UpdateCount);
            var similarImageCount = new Views.FileClassificationTileViewModel { Name = "Similar Images" };
            _eventAggreator.Subscribe<Tuple<long, long>>(Common.EventAggregatorConstants.BlurryImageCount, similarImageCount.UpdateCount);
            var blurryImage = new Views.FileClassificationTileViewModel { Name = "Blurry Images" };
            _eventAggreator.Subscribe<Tuple<long, long>>(Common.EventAggregatorConstants.ImageCount, blurryImage.UpdateCount);


            this.Items = new List<Views.FileClassificationTileViewModel>
            {
               allFiles,
               imageCount,
               documentCount,
               videoCount,
               duplicateCount,
               similarImageCount,
               blurryImage
            };
            return base.InitilizeAsync(data);
        }

    }
}
