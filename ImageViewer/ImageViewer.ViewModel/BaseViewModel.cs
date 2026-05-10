using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ImageViewer.ViewModel
{
    public abstract class BaseViewModel : ObservableObject
    {
        protected ServiceProvider ServiceCollection => CustomServiceCollection.CustomServiceCollection.ServiceProvider;
        private RelayCommand _navigateBack;
        public RelayCommand NavigationBackCommand { get { return _navigateBack; } protected set { _navigateBack = value; this.OnPropertyChanged(); } }

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

        public bool isViewUnassigned { get; set; } = false;

        public virtual void Initilize<T>(T data)
        {
            this.InitilizeAsync(data).GetAwaiter().GetResult();
            this.isViewUnassigned = false;
        }

        public  virtual async Task InitilizeAsync<T>(T data)
        {
            
        }
    }
}
