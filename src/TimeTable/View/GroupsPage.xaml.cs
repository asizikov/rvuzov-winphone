using System;
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
            if (NavigationContext.QueryString.TryGetValue(NavigationParameterName.Id, out parameter))
            {
                int universityId;
                if (Int32.TryParse(parameter, out universityId))
                {
                    DataContext = ViewModelLocator.GetGroupstPageViewModel(universityId);
                }
            }
        }
    }
}