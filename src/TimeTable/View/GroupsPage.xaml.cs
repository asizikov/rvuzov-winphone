using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using TimeTable.Utils;
using TimeTable.ViewModel;
using TimeTable.ViewModel.Services;

namespace TimeTable.View
{
    public partial class GroupsPage
    {
        public GroupsPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            string parameter;
            string rawuniversityId;
            if (NavigationContext.QueryString.TryGetValue(NavigationParameterName.Id, out parameter) &&
                NavigationContext.QueryString.TryGetValue(NavigationParameterName.UniversityId, out rawuniversityId))
            {
                int facultyId;
                int universityId;
                if (Int32.TryParse(parameter, out facultyId) &&
                    Int32.TryParse(rawuniversityId, out universityId))
                {
                    ViewModel =
                        ViewModelLocator.GetGroupsPageViewModel(facultyId, universityId, GetReason()) as
                            SearchViewModel;
                    DataContext = ViewModel;

                    if (State.Count > 0)
                    {
                        this.RestoreState(Search);
                        Search.Visibility = (Visibility) this.RestoreState(SEARCH_KEY);
                        this.RestoreState(Pivot);
                    }
                }
                else
                {
                    throw new ArgumentException();
                }
            }
            else
            {
                throw new ArgumentException();
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
                this.SaveState(Pivot);
            }
        }

        protected override void SetFocuse()
        {
            Search.Focus();
        }
    }
}