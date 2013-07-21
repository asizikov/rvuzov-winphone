using System;
using System.Windows.Navigation;
using JetBrains.Annotations;
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

            string updatedUri = null;

            if (_applicationSettings.GroupId != null)
            {
                updatedUri = string.Format("{0}?id={1}&name={2}", Pages.Lessons, _applicationSettings.GroupId, _applicationSettings.GroupName);
            }
            else if (_applicationSettings.UniversityId != null)
            {
                updatedUri = string.Format("{0}?id={1}", Pages.Groups, _applicationSettings.UniversityId);
            }
            else if (_applicationSettings.Role != null)
            {
                updatedUri = string.Format("{0}?id={1}", Pages.Universities, _applicationSettings.Role);
            }
            else
            {
                updatedUri = Pages.FirstPage;
            }

            return new Uri(updatedUri, UriKind.Relative);
        }
    }
}
