using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace MyAddressManageTool.View.Converter
{
    internal class StringTextboxConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((TextBox)value).Text;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            TextBox textBox = (TextBox)value;
            return textBox.Text;
        }
    }
}
