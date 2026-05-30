namespace ImageViewer.Service
{
    public interface IPublisher
    {
        void Publish<T>(string name, T data);
    }
}
