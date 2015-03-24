using System;
using System.Windows.Input;
using JetBrains.Annotations;
using TimeTable.Domain.Internal;
using TimeTable.Mvvm;
using TimeTable.ViewModel.Commands;
using TimeTable.ViewModel.OrganizationalStructure;
using TimeTable.ViewModel.Services;

namespace TimeTable.ViewModel.ApplicationLevel
{
    public class FirstPageViewModel : PageViewModel
    {
        private readonly Mvvm.Navigation.INavigationService _navigation;
        private readonly BaseApplicationSettings _applicationSettings;

        public FirstPageViewModel([NotNull] Mvvm.Navigation.INavigationService navigation,
                                  [NotNull] BaseApplicationSettings applicationSettings,
                                  [NotNull] FlurryPublisher flurryPublisher)
        {
            if (navigation == null) throw new ArgumentNullException("navigation");
            if (applicationSettings == null) throw new ArgumentNullException("applicationSettings");
            if (flurryPublisher == null) throw new ArgumentNullException("flurryPublisher");

            _navigation = navigation;
            _applicationSettings = applicationSettings;
            flurryPublisher.PublishPageLoadedSelectRole();

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
            _navigation.NavigateTo<UniversitiesPageViewModel, Reason>(Reason.Registration);
        }
    }
}