using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageViewer.Service.Interfaces
{
    public interface INavigationService
    {
        void Navigate(string frame,string navigation, object paramater);
        void NavigateBack(string frame);
        void RegisterNavigation(string frameName, Type navigation);
        void RegisterFrame(string frameName, Frame frame);
        Frame GetNavigationFrame(string frame);
    }
}
