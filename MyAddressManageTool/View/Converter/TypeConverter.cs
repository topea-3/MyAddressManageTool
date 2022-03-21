using MyAddressManageTool.MyApi;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MyAddressManageTool.View.Converter
{
    internal class TypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (null == value)
            {
                return "";
            }

            // string変換
            string valueStr = value.ToString() ?? "";
            string typeId = parameter.ToString() ?? throw new ArgumentNullException(nameof(parameter));

            // 区分値変換
            return TypeManager.GetValueName(typeId, valueStr) ?? value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (null == value)
            {
                return "";
            }

            // string変換
            string valueStr = value?.ToString() ?? "";
            string typeId = parameter.ToString() ?? throw new ArgumentNullException(nameof(parameter));

            return TypeManager.GetValue(typeId, valueStr) ?? "";
        }
    }
}
