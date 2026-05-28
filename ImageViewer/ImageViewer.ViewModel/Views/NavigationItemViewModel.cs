using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ImageViewer.ViewModel.Views
{
    public class NavigationItemViewModel:BaseViewModel
    {
        private Control _view;
        public Control View {  get { return _view; } set { _view = value; OnPropertyChanged(); }  }

        private string _name;
        public string Name { get { return _name; } set { _name = value; OnPropertyChanged(); }  }

        private string _tag;
        public string Tag { get { return _tag; } set { _tag = value; OnPropertyChanged(); } }
        private bool _useStandard;
        public bool UseStandard { get { return _useStandard; } set { _useStandard = value;UseCustom = !value ; OnPropertyChanged(); } }

        private bool _useCustom;
        public bool UseCustom { get { return _useCustom; } set { _useCustom  = value; UseStandard = !value; OnPropertyChanged(); }  }

        private RelayCommand _navigationCommand;
        public RelayCommand NavigationCommand { get { return _navigationCommand; } set { _navigationCommand = value; this.OnPropertyChanged(); } }

    }
}
