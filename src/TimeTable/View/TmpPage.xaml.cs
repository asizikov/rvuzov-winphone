using Microsoft.Phone.Controls;
using TimeTable.ViewModel;

namespace TimeTable.View
{
    public partial class TmpPage : PhoneApplicationPage
    {
        public TmpPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            DataContext = ViewModelLocator.GetTmpViewModel();
        }
    }
}