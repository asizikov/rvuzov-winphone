using Microsoft.Phone.Controls;
using TimeTable.Mvvm.Navigation;
using TimeTable.ViewModel;
using TimeTable.ViewModel.ApplicationLevel;

namespace TimeTable.View
{
    [DependsOnViewModel(typeof(FirstPageViewModel))]
    public partial class FirstPage : PhoneApplicationPage
    {
        public FirstPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            DataContext = ViewModelLocator.GetFirstPageViewModel();
        }
    }
}