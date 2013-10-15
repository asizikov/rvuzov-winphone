using System;
using System.Windows.Navigation;
using JetBrains.Annotations;
using TimeTable.Model;
using TimeTable.ViewModel.Services;

namespace TimeTable
{
    internal sealed class TimeTableUriMapper : UriMapperBase
    {
        private readonly BaseApplicationSettings _applicationSettings;

        public TimeTableUriMapper([NotNull] BaseApplicationSettings applicationSettings)
        {
            if (applicationSettings == null) throw new ArgumentNullException("applicationSettings");
            _applicationSettings = applicationSettings;
        }

        public override Uri MapUri(Uri uri)
        {
            if (!uri.OriginalString.Contains(Pages.EntryPoint))
            {
                return uri;
            }

            string updatedUri;

            if (_applicationSettings.Me.DefaultGroup != null || _applicationSettings.Me.Teacher != null)
            {
                var isTeacher = _applicationSettings.Me.Teacher != null;
                updatedUri = string.Format("{0}?id={1}&is_teacher={2}&university_id={3}&faculty_id={4}", 
                    Pages.Lessons, 
                    isTeacher ? _applicationSettings.Me.Teacher.Id : _applicationSettings.Me.DefaultGroup.Id,
                    isTeacher, 
                    _applicationSettings.Me.University.Id, 
                    _applicationSettings.Me.Faculty);
            }
            else if (_applicationSettings.Me.Faculty != null)
            {
                updatedUri = string.Format("{0}?id={1}&university_id={2}", Pages.Groups, _applicationSettings.Me.Faculty.Id, _applicationSettings.Me.University.Id);
            }
            else if (_applicationSettings.Me.University != null)
            {
                updatedUri = string.Format("{0}?id={1}", Pages.Faculties, _applicationSettings.Me.University.Id);
            }
            else if (_applicationSettings.Me.Role != UserRole.None)
            {
                updatedUri = string.Format("{0}?id={1}", Pages.Universities, _applicationSettings.Me.Role);
            }
            else
            {
                updatedUri = Pages.FirstPage;
            }

            return new Uri(updatedUri, UriKind.Relative);
        }
    }
}
