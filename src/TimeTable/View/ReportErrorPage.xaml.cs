using System;
using System.Windows.Navigation;
using TimeTable.ViewModel;
using TimeTable.ViewModel.Services;

namespace TimeTable.View
{
    public partial class ReportErrorPage
    {
        public ReportErrorPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var id = string.Empty;
            var lessonId = string.Empty;
            var isTeacher = string.Empty;
            NavigationContext.QueryString.TryGetValue(NavigationParameterName.Id, out id);
            NavigationContext.QueryString.TryGetValue(NavigationParameterName.IsTeacher, out isTeacher);
            NavigationContext.QueryString.TryGetValue(NavigationParameterName.LessonId, out lessonId);
            
            DataContext = ViewModelLocator.GetReportErrorViewModel(Int32.Parse(id), Int32.Parse(lessonId), Boolean.Parse(isTeacher));
        }
    }
}