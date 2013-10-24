using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using TimeTable.Utils;
using TimeTable.ViewModel;

namespace TimeTable.View
{
    public partial class UniversitiesPage
    {
        public UniversitiesPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ViewModel = ViewModelLocator.GetUniversitiesViewModel(GetReason()) as SearchViewModel;
            DataContext = ViewModel;

            if (State.Count > 0)
            {
                this.RestoreState(Search);
                Search.Visibility = (Visibility)this.RestoreState(SEARCH_KEY);
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
                this.SaveState(SEARCH_KEY, Search.Visibility);
            }
        }

    }
}