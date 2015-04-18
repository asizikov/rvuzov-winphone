using System;
using System.Diagnostics;
using System.Windows.Navigation;
using JetBrains.Annotations;
using TimeTable.Domain.Internal;
using TimeTable.Mvvm.Navigation;
using TimeTable.View;
using TimeTable.ViewModel.OrganizationalStructure;
using TimeTable.ViewModel.Services;
using TimeTable.ViewModel.WeekOverview;

namespace TimeTable
{
    [UsedImplicitly]
    internal sealed class TimeTableUriMapper : UriMapperBase
    {
        private readonly BaseApplicationSettings _applicationSettings;
        private INavigationService NavigationService { get; set; }

        public TimeTableUriMapper([NotNull] BaseApplicationSettings applicationSettings,
                                  [NotNull] INavigationService navigationService)
        {
            if (applicationSettings == null) throw new ArgumentNullException("applicationSettings");
            if (navigationService == null) throw new ArgumentNullException("navigationService");
            _applicationSettings = applicationSettings;
            NavigationService = navigationService;
        }

        public override Uri MapUri(Uri uri)
        {
            Debug.WriteLine("UriMapper:MapUri for {0}", uri.OriginalString);
            if (!uri.OriginalString.Contains("/View/EntryPoint.xaml"))
            {
                return uri;
            }

            if (_applicationSettings.Me.DefaultGroup != null || _applicationSettings.Me.Teacher != null)
            {
                var isTeacher = _applicationSettings.Me.Teacher != null;
                var navigationParameter = new LessonsNavigationParameter
                {
                    Id = isTeacher ? _applicationSettings.Me.Teacher.Id : _applicationSettings.Me.DefaultGroup.Id,
                    IsTeacher = isTeacher,
                    UniversityId = _applicationSettings.Me.University.Id,
                    FacultyId = _applicationSettings.Me.Faculty.Id
                };
                return NavigationService.GetUri<NewLessonsPage, LessonsNavigationParameter>(navigationParameter);
            }

            var navigationFlow = new NavigationFlow();
            if (_applicationSettings.Me.Faculty != null)
            {
                navigationFlow.FacultyId = _applicationSettings.Me.Faculty.Id;
                navigationFlow.UniversityId = _applicationSettings.Me.University.Id;
                navigationFlow.UniversityName = _applicationSettings.Me.University.ShortName;
                navigationFlow.FacultyName = _applicationSettings.Me.Faculty.Title;
                return NavigationService.GetUri<NewGroupsPage, NavigationFlow>(navigationFlow);
            }
            if (_applicationSettings.Me.University != null)
            {
                navigationFlow.UniversityId = _applicationSettings.Me.University.Id;
                navigationFlow.UniversityName = _applicationSettings.Me.University.ShortName;
                return NavigationService.GetUri<NewFacultiesPage, NavigationFlow>(navigationFlow);
            }
            if (_applicationSettings.Me.Role != UserRole.None)
            {
                return NavigationService.GetUri<NewUniversitiesPage, Reason>(Reason.Registration);
            }
            return NavigationService.GetUri<FirstPage>();
        }
    }
}