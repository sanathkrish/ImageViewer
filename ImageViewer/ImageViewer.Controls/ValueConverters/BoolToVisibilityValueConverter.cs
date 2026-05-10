using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageViewer.Controls.ValueConverters
{
    public class BoolToVisibilityValueConverter: IValueConverter
    {
        public BoolToVisibilityValueConverter()
        {
            
        }
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool boolValue)
            {
                return boolValue ? Microsoft.UI.Xaml.Visibility.Visible : Microsoft.UI.Xaml.Visibility.Collapsed;
            }
            return Microsoft.UI.Xaml.Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is Microsoft.UI.Xaml.Visibility visibility)
            {
                return visibility == Microsoft.UI.Xaml.Visibility.Visible;
            }
            return false;
        }
    }
}