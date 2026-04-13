using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageViewer.ViewModel
{
    public abstract class BaseViewModel : ObservableObject
    {
        protected ServiceProvider ServiceCollection => CustomServiceCollection.CustomServiceCollection.ServiceProvider;
        private string title = string.Empty;
        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                OnPropertyChanged();
            }
        }

        public virtual void Initilize<T>(T data)
        {
            this.InitilizeAsync(data).GetAwaiter().GetResult();
        }

        public  virtual async Task InitilizeAsync<T>(T data)
        {
            
        }
    }
}
