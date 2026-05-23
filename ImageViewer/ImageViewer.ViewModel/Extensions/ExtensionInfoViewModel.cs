using ImageViewer.Model;
using ImageViewer.ViewModel.Common;
using ImageViewer.ViewModel.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageViewer.ViewModel.Extensions
{
    public class ExtensionInfoViewModel : BaseViewModel
    {
        public ExtensionInfoViewModel() 
        {
            _eventAggregator.Subscribe<ExtensionInfo>(EventAggregatorConstants.ExntensionInfoUpdated, (extension) =>
            {
                if (Extension == extension.Extension)
                {
                    Extensions.Add(extension.Extension);
                    Count++;
                }
            });
        }
        private EventAggreator _eventAggregator = EventAggreator.Instance;
        private ObservableCollection<string> _extensions = new ObservableCollection<string>();

        public ObservableCollection<string> Extensions
        {
            get { return _extensions; }
            set
            {
                _extensions = value;
                OnPropertyChanged();
            }
        }

        private long _count;
        public long Count
        {
            get { return _count; }
            set
            {
                _count = value;
                OnPropertyChanged();
            }
        }

        private string _extension;
        public string Extension
        {
            get { return _extension; }
            set
            {
                _extension = value;
                OnPropertyChanged();
            }
        }
    }
}
