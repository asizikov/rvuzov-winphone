using System;
using System.Windows;
using System.Windows.Controls;
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

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
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
    }
}