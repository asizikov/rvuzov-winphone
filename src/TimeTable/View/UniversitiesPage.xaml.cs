using TimeTable.ViewModel;

namespace TimeTable.View
{
    public partial class UniversitiesPage
    {
        public UniversitiesPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            DataContext = ViewModelLocator.GetUniversitiesViewModel();
        }
    }
}