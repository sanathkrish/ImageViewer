using ImageViewer.Service.Interfaces;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageViewer.Controls.Navigation
{
    public class NavigationService : INavigationService
    {
        private Dictionary<string, Frame>    _frames = new Dictionary<string, Frame>();
        private Dictionary<string,Dictionary<string,Type>> _pages = new Dictionary<string, Dictionary<string,Type>>();

        public Frame GetNavigationFrame(string frame)
        {
            return _frames.GetValueOrDefault(frame);
        }

        public void Navigate(string frame,string navigation, object paramater)
        {
          if(_frames.ContainsKey(frame) && _pages.ContainsKey(frame) && _pages[frame].ContainsKey(navigation))
          {
              _frames[frame].Navigate(_pages[frame][navigation], paramater);
          }
        }

        public void NavigateBack(string frame)
        {
           if(_frames.ContainsKey(frame) && _frames[frame].CanGoBack)
            {
                _frames[frame].GoBack();
            }
        }

        public void RegisterFrame(string frameName, Frame frame)
        {
            if (!_frames.ContainsKey(frameName))
            {
                _frames.Add(frameName, frame);
            }
        }

        public void RegisterNavigation(string frameName, Type navigation)
        {
            if (!_pages.ContainsKey(frameName))
            {
                _pages.Add(frameName, new Dictionary<string, Type>());
                _pages[frameName].Add(navigation.Name, navigation);
            }else
            {
                if(_pages.ContainsKey(frameName) && !_pages[frameName].ContainsKey(navigation.Name))
                {
                    _pages[frameName].Add(navigation.Name, navigation);
                }
            }
        }
    }
}
