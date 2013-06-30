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

            string parameter;
            if (NavigationContext.QueryString.TryGetValue(NavigationParameterName.Id, out parameter))
            {
                int groupId;
                if (Int32.TryParse(parameter, out groupId))
                {
                    DataContext = ViewModelLocator.GetLessonsViewModel(groupId);
                }
            }
        }
    }
}