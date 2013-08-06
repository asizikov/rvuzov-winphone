using System;
using System.Globalization;
using System.Windows.Data;

namespace TimeTable.Converters
{
    public class NumberToWeekDayConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var dayNumber = (int) value;
            if (dayNumber < 1 || dayNumber > 7)
            {
                throw new ArgumentException("Day number value is out of range");
            }
            return CultureInfo.CurrentCulture.DateTimeFormat.DayNames[dayNumber];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
