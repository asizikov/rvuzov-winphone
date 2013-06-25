using System;
using System.Windows.Input;
using JetBrains.Annotations;
using TimeTable.Model.User;
using TimeTable.ViewModel.Commands;
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
            _navigation.GoToPage(Pages.Universites);
        }
    }
}
