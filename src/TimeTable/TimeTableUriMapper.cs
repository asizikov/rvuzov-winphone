using System;
using System.Windows.Navigation;
using JetBrains.Annotations;
using TimeTable.Domain.Internal;
using TimeTable.Mvvm.Navigation;
using TimeTable.ViewModel.ApplicationLevel;
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
            if (!uri.OriginalString.Contains("/View/EntryPoint.xaml"))
            {
                return uri;
            }

            var navigationFlow = new NavigationFlow();
            if (_applicationSettings.Me.DefaultGroup != null || _applicationSettings.Me.Teacher != null)
            {
                var isTeacher = _applicationSettings.Me.Teacher != null;
                var navigationParameter = new LessonsNavigationParameter
                {
                    Id=isTeacher ? _applicationSettings.Me.Teacher.Id : _applicationSettings.Me.DefaultGroup.Id,
                    IsTeacher = isTeacher,
                    UniversityId = _applicationSettings.Me.University.Id,
                    FacultyId = _applicationSettings.Me.Faculty.Id
                };
                return NavigationService.GetUri<LessonsPageViewModel, LessonsNavigationParameter>(navigationParameter);
            }
            if (_applicationSettings.Me.Faculty != null)
            {
                navigationFlow.FacultyId = _applicationSettings.Me.Faculty.Id;
                navigationFlow.UniversityId = _applicationSettings.Me.University.Id;
                return NavigationService.GetUri<GroupPageViewModel, NavigationFlow>(navigationFlow);
            }
            if (_applicationSettings.Me.University != null)
            {
                navigationFlow.UniversityId = _applicationSettings.Me.University.Id;
                return NavigationService.GetUri<FacultiesPageViewModel, NavigationFlow>(navigationFlow );
            }
            if (_applicationSettings.Me.Role != UserRole.None)
            {
                return NavigationService.GetUri<UniversitiesPageViewModel, Reason>(Reason.Registration);
            }
            return NavigationService.GetUri<FirstPageViewModel>();
        }
    }
}
