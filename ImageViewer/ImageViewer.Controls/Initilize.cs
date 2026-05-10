using ImageViewer.Controls.Elements;
using ImageViewer.Controls.Navigation;
using ImageViewer.Service.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageViewer.Controls
{
    public static class Initilize
    {
        public static void InitilizeApp()
        {
            ViewModel.CustomServiceCollection.CustomServiceCollection.Initilize();
            var _ = ViewModel.CustomServiceCollection.CustomServiceCollection.GetServiceCollection();
            _.AddTransient<BaseItemView> ();
            _.AddSingleton<INavigationService, NavigationService>();
        }

    }
}
