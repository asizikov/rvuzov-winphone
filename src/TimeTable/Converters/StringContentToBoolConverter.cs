using System;
using System.Globalization;
using System.Windows.Data;

namespace TimeTable.Converters
{
    public class StringContentToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var str = value as string;
            return (!string.IsNullOrWhiteSpace(str));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}