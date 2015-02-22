using System;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace TimeTable.Converters
{
    public class AnyCountToVisibilityConverter : IValueConverter
    {
        private Visibility _anyVisibility = Visibility.Visible;
        public Visibility AnyVisibility
        {
            get { return _anyVisibility; }
            set { _anyVisibility = value; }
        }

        private Visibility _emptyVisibility = Visibility.Collapsed;
        public Visibility EmptyVisibility
        {
            get { return _emptyVisibility; }
            set { _emptyVisibility = value; }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var iList = value as IList;
            if (iList == null)
            {
                Debug.WriteLine("AnyCountToVisibilityConverter didn't get an IList as a parameter");
                iList = new object[]{};
            }

            return iList.Count > 0 ? AnyVisibility : EmptyVisibility;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("AnyCountToVisibilityConverter.ConvertBack");
        }
    }
}