using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageViewer.ViewModel.Common
{
    public static class EventAggregatorConstants
    {
        public const string ExntensionInfoUpdated = "ExtensionInfoUpdated";
        public const string AllFilesCount = "all_files_count";
        public const string ImageCount = "image_count";
        public const string VideoCount = "video_count";
        public const string DocumentCount = "document_count";
        public const string DuplicateCount = "duplicate_count";
        public const string SimilarImageCount = "similar_image_count";
        public const string BlurryImageCount = "blurry_image_count";
    }
}
