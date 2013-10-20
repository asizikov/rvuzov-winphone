using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using TimeTable.ViewModel;

namespace TimeTable.View
{
    public partial class ReportErrorPage : PhoneApplicationPage
    {
        public ReportErrorPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            DataContext = ViewModelLocator.GetReportErrorViewModel();
        }
    }
}