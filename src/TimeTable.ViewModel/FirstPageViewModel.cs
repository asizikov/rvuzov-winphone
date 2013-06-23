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
        private readonly INavigationService navigation;

        public FirstPageViewModel([NotNull] INavigationService navigation)
        {
            if (navigation == null) throw new ArgumentNullException("navigation");
            this.navigation = navigation;
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
            //todo: update settings
            navigation.GoToPage(Pages.Universities);
        }
    }
}
