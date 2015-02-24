using System.Windows.Navigation;
using TimeTable.Mvvm.Navigation;
using TimeTable.ViewModel;
using TimeTable.ViewModel.ApplicationLevel;

namespace TimeTable.View
{
    [DependsOnViewModel(typeof(AboutViewModel))]
    public partial class AboutPage 
    {
        public AboutPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            DataContext = ViewModelLocator.GetAboutViewModel();
        }
    }
}