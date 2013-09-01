using System;
using System.Globalization;
using System.Windows.Data;
using TimeTable.Resources;

namespace TimeTable.Converters
{
    public class WeekNumberToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return String.Empty;
            var number = (int) value;
            return string.Format(Strings.WeekNumber, number);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}