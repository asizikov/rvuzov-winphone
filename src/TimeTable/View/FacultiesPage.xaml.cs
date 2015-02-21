using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using TimeTable.Utils;
using TimeTable.ViewModel;
using TimeTable.ViewModel.Services;

namespace TimeTable.View
{
    public partial class FacultiesPage
    {
        public FacultiesPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            string parameter;
            if (NavigationContext.QueryString.TryGetValue(NavigationParameterName.Id, out parameter))
            {
                int universityId;
                if (Int32.TryParse(parameter, out universityId))
                {
                    ViewModel = ViewModelLocator.GetFacultiesPageViewModel(universityId, GetReason()) as SearchViewModel;
                    DataContext = ViewModel;
                }
            }
            DataContext = ViewModel;

            if (State.Count > 0)
            {
                this.RestoreState(Search);
                Search.Visibility = (Visibility) this.RestoreState(SEARCH_KEY);
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

        protected override void SetFocus()
        {
            Search.Focus();
        }
    }
}