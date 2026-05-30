using ImageViewer.ViewModel.Events;
using ImageViewer.Service;

namespace ImageViewer.ViewModel
{
    public class EventPublisherForDI : IPublisher
    {
        public void Publish<T>(string name, T data)
        {
            try { EventAggreator.Instance.Publish(name, data); } catch { }
        }
    }
}
