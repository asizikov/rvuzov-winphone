using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using TimeTable.Mvvm.Navigation;
using TimeTable.Utils;
using TimeTable.ViewModel;
using TimeTable.ViewModel.OrganizationalStructure;

namespace TimeTable.View
{
    [DependsOnViewModel(typeof (FacultiesPageViewModel))]
    public partial class FacultiesPage
    {
        public FacultiesPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var navigationContext = NavigationContext.QueryString.RestoreContext<NavigationFlow>();

            ViewModel = ViewModelLocator.GetFacultiesPageViewModel(navigationContext.Body) as ISearchViewModel;
            DataContext = ViewModel;

            if (State.Count > 0)
            {
                this.RestoreState(Search);
                Search.Visibility = (Visibility) this.RestoreState(SearchKey);
            }
        }

        private void Search_GotFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null) textBox.SelectAll();
        }

        protected override void SaveState(NavigatingCancelEventArgs e)
        {
            if (this.ShouldTombstone(e))
            {
                this.SaveState(Search);
                this.SaveState(SearchKey, Search.Visibility);
            }
        }

        protected override void SetFocus()
        {
            Search.Focus();
        }
    }
}