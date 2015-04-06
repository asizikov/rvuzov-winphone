using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using TimeTable.Mvvm.Navigation;
using TimeTable.Utils;
using TimeTable.ViewModel;
using TimeTable.ViewModel.OrganizationalStructure;

namespace TimeTable.View
{
    [DependsOnViewModel(typeof (GroupPageViewModel))]
    public partial class NewGroupsPage
    {
        public NewGroupsPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var navigationContext = NavigationContext.QueryString.RestoreContext<NavigationFlow>();

            ViewModel =
                ViewModelLocator.GetGroupsPageViewModel(navigationContext.Body) as
                    ISearchViewModel;

            if (ViewModel != null)
            {
                ViewModel.OnLock += OnLock;
            }
            DataContext = ViewModel;

            if (State.Count > 0)
            {
//                this.RestoreState(Search);
//                Search.Visibility = (Visibility) this.RestoreState(SearchKey);
//                this.RestoreState(Pivot);
            }
        }

        private void OnLock(bool state)
        {
//            Pivot.IsLocked = state;
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
//                this.SaveState(Search);
//                this.SaveState(SearchKey, Search.Visibility);
//                this.SaveState(Pivot);
            }
        }

        protected override void SetFocus()
        {
//            Search.Focus();
        }

        protected override void OnLeave()
        {
            if (ViewModel != null)
            {
// ReSharper disable once DelegateSubtraction
                ViewModel.OnLock -= OnLock;
            }
            base.OnLeave();
        }
    }
}