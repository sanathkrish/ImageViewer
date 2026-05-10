using AutoMapper;
using ImageViewer.ViewModel.File;
using ImageViewer.ViewModel.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ImageViewer.Controls.Elements
{
    public sealed partial class ItemView : UserControl
    {
        public ItemView()
        {
            InitializeComponent();
            //DataContextChanged += ItemView_DataContextChanged;
        }

        //private void ItemView_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        //{
        //    baseFile = args.NewValue as BaseItemViewModel;
        //    if (baseFile != null)
        //    {
        //       var element = ViewModel.CustomServiceCollection.CustomServiceCollection.ServiceProvider.GetService<BaseItemView>();
        //        this.base_item.Children.Add(element);
        //        element.Initilize(baseFile);
        //        baseFile.RetriggerProperty();
        //        this.ApplyTemplate();
        //    }
        //}

        public BaseFileViewModel Vm
        {
            get => (BaseFileViewModel)GetValue(VModel);
            set => SetValue(VModel, value);
        }

        public static readonly DependencyProperty VModel =
            DependencyProperty.Register(
                nameof(Vm),
                typeof(BaseFileViewModel),
                typeof(ItemView),
      new PropertyMetadata(null, OnVmChanged));

        private static void OnVmChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (ItemView)d;
            var newValue = e.NewValue;
            var mapper = ViewModel.CustomServiceCollection.CustomServiceCollection.ServiceProvider.GetService<IMapper>();
            if (control != null && control.base_item != null)
            {
                control.base_item.Initilize(mapper.Map< BaseItemViewModel>(newValue));   
                ((BaseItemViewModel)control.base_item.DataContext)?.RetriggerProperty();
            }
            // Do something when value changes
        }
    }
}
