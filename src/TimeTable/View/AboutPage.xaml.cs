using System.Windows.Navigation;
using TimeTable.ViewModel;

namespace TimeTable.View
{
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