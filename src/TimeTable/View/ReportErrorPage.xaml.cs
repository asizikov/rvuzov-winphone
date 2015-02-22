using System;
using System.Windows.Navigation;
using TimeTable.Utils;
using TimeTable.ViewModel;
using TimeTable.ViewModel.Services;

namespace TimeTable.View
{
    public partial class ReportErrorPage
    {
        private string _errotText;
        private const string ERROR_TEXT_KEY = "LastSearchKey";

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
            
//            DataContext = ViewModelLocator.GetReportErrorViewModel(Int32.Parse(id), Int32.Parse(lessonId), Boolean.Parse(isTeacher));

            if (State.Count > 0)
            {
                this.RestoreState(ErrorTextTextBox);
                _errotText = this.RestoreState(ERROR_TEXT_KEY) as string;
            }
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            State.Clear();
            if (this.ShouldTombstone(e))
            {
                this.SaveState(ErrorTextTextBox);
                if (!string.IsNullOrEmpty(_errotText))
                {
                    this.SaveState(ERROR_TEXT_KEY, _errotText);
                }
            }
            _errotText = string.Empty;
        }
    }
}