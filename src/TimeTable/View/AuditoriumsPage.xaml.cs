using System.Windows.Navigation;
using TimeTable.Mvvm.Navigation;
using TimeTable.ViewModel;
using TimeTable.ViewModel.OrganizationalStructure;

namespace TimeTable.View
{
    [DependsOnViewModel(typeof (AuditoriumViewModel))]
    public partial class AuditoriumsPage
    {
        public AuditoriumsPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var navigationContext = NavigationContext.QueryString.RestoreContext<AuditoriumNavigationParameter>();

            DataContext = ViewModelLocator.GetAuditoriumViewModel(navigationContext.Body);
        }
    }
}