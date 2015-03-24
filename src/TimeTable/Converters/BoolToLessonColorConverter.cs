using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace TimeTable.Converters
{
    public class BoolToLessonColorConverter : IValueConverter
    {
        private readonly SolidColorBrush _currentBrush =
            new SolidColorBrush((Color) Application.Current.Resources["PhoneAccentColor"]);

        private readonly SolidColorBrush _overageBrush = new SolidColorBrush(Color.FromArgb(255, 76, 109, 167));

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((bool) value) ? _currentBrush : _overageBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}