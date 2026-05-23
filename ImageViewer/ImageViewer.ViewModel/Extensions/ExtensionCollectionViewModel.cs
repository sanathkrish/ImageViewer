using ImageViewer.Model;
using ImageViewer.Service.File;
using ImageViewer.ViewModel.Common;
using ImageViewer.ViewModel.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Dispatching;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ImageViewer.ViewModel.Extensions
{
    public class ExtensionCollectionViewModel:BaseViewModel
    {

        public ExtensionCollectionViewModel() {
        this._extensionService = CustomServiceCollection.CustomServiceCollection.ServiceProvider.GetService<ExtensionService>();
        }
        public DispatcherQueue dispatcherQueue;
        private ExtensionService _extensionService;
        private ObservableCollection<ExtensionInfoViewModel> _extensions = new ObservableCollection<ExtensionInfoViewModel>();
        public ObservableCollection<ExtensionInfoViewModel> Extensions
        {
            get { return _extensions; }
            set { _extensions = value; }
        }

        public override Task InitilizeAsync<String>(String data)
        {
           return Task.Run(() =>
            {
                var extensions = _extensionService.GetExtensions(data.ToString(),(eventInfo)=> VerifyAndLoadExtensions(eventInfo));
            });
        }

        private void VerifyAndLoadExtensions(ExtensionInfo extension)
        {
            this.dispatcherQueue.TryEnqueue(() =>
             {
                 var exist = Extensions.Any(e => e.Extension == extension.Extension);
                 if (!exist)
                 {
                     var model = new ExtensionInfoViewModel();
                     model.Extension = extension.Extension;
                     Extensions.Add(model);
                 }
                 EventAggreator.Instance.Publish(EventAggregatorConstants.ExntensionInfoUpdated, extension);
             });
           
        }
    }
}
