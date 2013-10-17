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
        #region AnyVisibility
        private Visibility _anyVisibility = Visibility.Visible;
        public Visibility AnyVisibility
        {
            get { return _anyVisibility; }
            set { _anyVisibility = value; }
        }
        #endregion

        #region EmptyVisibility
        private Visibility _emptyVisibility = Visibility.Collapsed;
        public Visibility EmptyVisibility
        {
            get { return _emptyVisibility; }
            set { _emptyVisibility = value; }
        }
        #endregion

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var iList = value as IList;
            if (iList == null)
            {
                Debug.WriteLine("В AnyCountToVisibilityConverter передан не IList");
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