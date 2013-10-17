using System;
using System.Threading;
using System.Windows.Input;
using JetBrains.Annotations;
using TimeTable.Model;
using TimeTable.ViewModel.Commands;
using TimeTable.ViewModel.Data;
using TimeTable.ViewModel.Services;

namespace TimeTable.ViewModel
{
    public class FirstPageViewModel : BaseViewModel
    {
        private readonly INavigationService _navigation;
        private readonly BaseApplicationSettings _applicationSettings;
        private readonly AsyncDataProvider _asyncDataProvider;

        public FirstPageViewModel([NotNull] INavigationService navigation,
            [NotNull] BaseApplicationSettings applicationSettings,
            [NotNull] AsyncDataProvider asyncDataProvider)
        {
            if (navigation == null) throw new ArgumentNullException("navigation");
            if (applicationSettings == null) throw new ArgumentNullException("applicationSettings");
            if (asyncDataProvider == null) throw new ArgumentNullException("asyncDataProvider");

            _navigation = navigation;
            _applicationSettings = applicationSettings;
            _asyncDataProvider = asyncDataProvider;

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
            _applicationSettings.Me.Role = role;
            _navigation.GoToPage(Pages.Universities);
        }
    }
}