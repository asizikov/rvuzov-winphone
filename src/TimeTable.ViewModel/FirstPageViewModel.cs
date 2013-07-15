using System;
using System.Windows.Input;
using JetBrains.Annotations;
using TimeTable.ViewModel.Commands;
using TimeTable.ViewModel.Enums;
using TimeTable.ViewModel.Services;

namespace TimeTable.ViewModel
{
    public class FirstPageViewModel : BaseViewModel
    {
        private readonly INavigationService _navigation;
        private readonly BaseApplicationSettings _applicationSettings;

        public FirstPageViewModel([NotNull] INavigationService navigation,
                                  [NotNull] BaseApplicationSettings applicationSettings)
        {
            if (navigation == null) throw new ArgumentNullException("navigation");
            if (applicationSettings == null) throw new ArgumentNullException("applicationSettings");

            _navigation = navigation;
            _applicationSettings = applicationSettings;

            InitCommands();

            TryNavigateToUserPage();
        }

        private void TryNavigateToUserPage()
        {
            if (!_applicationSettings.FirstLoad)
                return;

            string userPage = null;

            if (_applicationSettings.GroupId != null)
            {
                userPage = string.Format("{0}?id={1}", Pages.Lessons, _applicationSettings.GroupId);
            }
            else if (_applicationSettings.UniversityId != null)
            {
                userPage = string.Format("{0}?id={1}", Pages.Groups, _applicationSettings.UniversityId);
            }
            else if (_applicationSettings.Role != null)
            {
                userPage = string.Format("{0}?id={1}", Pages.Universities, _applicationSettings.Role);
            }

            if (userPage != null)
            {
                _navigation.GoToPage(userPage);
            }
            _applicationSettings.FirstLoad = false;
        }

        [UsedImplicitly(ImplicitUseKindFlags.Access)]
        public ICommand ImStudentCommand { get; private set; }

        [UsedImplicitly(ImplicitUseKindFlags.Access)]
        public ICommand ImTeacherCommand { get; private set; }

        private void InitCommands()
        {
            ImStudentCommand = new SimpleCommand(() => SaveUserRoleAndNavigateToNextPage(UserRole.Student));
            ImTeacherCommand = new SimpleCommand(() => SaveUserRoleAndNavigateToNextPage(UserRole.Teacher));
        }

        private void SaveUserRoleAndNavigateToNextPage(UserRole role)
        {
            _applicationSettings.Role = role;
            _navigation.GoToPage(Pages.Universities);
        }
    }
}