using System;
using System.Diagnostics;
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
            string rawUniversityId;
            if (NavigationContext.QueryString.TryGetValue(NavigationParameterName.Id, out rawGroupId)
                && NavigationContext.QueryString.TryGetValue(NavigationParameterName.IsTeacher, out rawIsTeacher)
                && NavigationContext.QueryString.TryGetValue(NavigationParameterName.UniversityId, out rawUniversityId))
            {
                int id;
                if (Int32.TryParse(rawGroupId, out id))
                {
                    DataContext = ViewModelLocator.GetLessonsViewModel(id, Boolean.Parse(rawIsTeacher),
                        Int32.Parse(rawUniversityId));
                }
            }
            else
            {
                Debug.WriteLine("LessonsPage::failed to get parameters from QueryString");
            }
        }
    }
}