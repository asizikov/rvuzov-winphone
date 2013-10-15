using System.Windows.Navigation;
using TimeTable.ViewModel;

namespace TimeTable.View
{
    public partial class FavoritesPage
    {
        public FavoritesPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            DataContext = ViewModelLocator.GetFavoritesViewModel();
        }
    }
}