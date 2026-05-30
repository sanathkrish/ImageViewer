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

        public FileClassificationTileCollection()
        {
            this.InitilizeAsync<string>(string.Empty).ConfigureAwait(false);
        }

        public override Task InitilizeAsync<String>(String data)
        {
            this.Items = new List<Views.FileClassificationTileViewModel>
            {
                new Views.FileClassificationTileViewModel{ Name = "All Files" },
                new Views.FileClassificationTileViewModel{ Name = "Images" },
                new Views.FileClassificationTileViewModel{ Name = "Documents" },
                new Views.FileClassificationTileViewModel{ Name = "Videos" },
                new Views.FileClassificationTileViewModel{ Name = "Duplicate" },
                new Views.FileClassificationTileViewModel{ Name = "Similar Images" },
                new Views.FileClassificationTileViewModel{ Name = "Blurry Images" }
            };
            return base.InitilizeAsync(data);
        }
    }
}
