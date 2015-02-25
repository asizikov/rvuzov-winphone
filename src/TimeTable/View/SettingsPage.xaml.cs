using TimeTable.Mvvm.Navigation;
using TimeTable.ViewModel;
using TimeTable.ViewModel.ApplicationLevel;

namespace TimeTable.View
{
    [DependsOnViewModel(typeof(SettingsViewModel))]
    public partial class SettingsPage
    {
        public SettingsPage()
        {
            InitializeComponent();
        }
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            DataContext = ViewModelLocator.GetSettingsViewModel();
        }
    }
}