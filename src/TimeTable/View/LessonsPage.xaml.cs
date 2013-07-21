using System;
using TimeTable.ViewModel;
using TimeTable.ViewModel.Services;

namespace TimeTable.View
{
    public partial class LessonsPage
    {
        public LessonsPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            string rawGroupId;
            string rawGroupName;
            if (NavigationContext.QueryString.TryGetValue(NavigationParameterName.Id, out rawGroupId)
                && NavigationContext.QueryString.TryGetValue(NavigationParameterName.Name, out rawGroupName))
            {
                int groupId;
                if (Int32.TryParse(rawGroupId, out groupId))
                {
                    DataContext = ViewModelLocator.GetLessonsViewModel(groupId, Uri.UnescapeDataString(rawGroupName));
                }
            }
        }
    }
}