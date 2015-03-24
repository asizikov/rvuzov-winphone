using System.Windows.Navigation;
using TimeTable.Mvvm.Navigation;
using TimeTable.ViewModel;
using TimeTable.ViewModel.FavoritedTimeTables;

namespace TimeTable.View
{
    [DependsOnViewModel(typeof (FavoritesViewModel))]
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