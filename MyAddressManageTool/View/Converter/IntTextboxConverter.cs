using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace MyAddressManageTool.View.Converter
{
    internal class IntTextboxConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            TextBox textBox = new();
            textBox.Text = value.ToString();
            return textBox;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            TextBox textBox = (TextBox)value;
            try
            {
                return int.Parse(textBox.Text);
            }
            catch (FormatException e)
            {
                _ = MessageBox.Show("整数値で入力してください。", "フォーマットエラー", MessageBoxButton.OK, MessageBoxImage.None);
            }
            throw new NotImplementedException();
        }
    }
}
