using System.Windows;
using System.Windows.Controls;

namespace TimeTable.Controls
{
    public partial class TimeTableSmallHeader : UserControl
    {
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof (string), typeof (TimeTableSmallHeader), null);

        public string Text
        {
            get { return GetValue(TextProperty) as string; }
            set { SetValue(TextProperty, value); }
        }

        public TimeTableSmallHeader()
        {
            InitializeComponent();
        }
    }
}