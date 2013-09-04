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
            string rawIsTeacher;
            if (NavigationContext.QueryString.TryGetValue(NavigationParameterName.Id, out rawGroupId)
                && NavigationContext.QueryString.TryGetValue(NavigationParameterName.IsTeacher, out rawIsTeacher))
            {
                int id;
                if (Int32.TryParse(rawGroupId, out id))
                {
                    DataContext = ViewModelLocator.GetLessonsViewModel(id, Boolean.Parse(rawIsTeacher));
                }
            }
        }
    }
}