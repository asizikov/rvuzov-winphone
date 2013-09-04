using System;
using System.Windows.Input;
using JetBrains.Annotations;
using TimeTable.ViewModel.Commands;
using TimeTable.ViewModel.Services;

namespace TimeTable.Commands
{
    class CommandsFactory : ICommandFactory
    {
        private readonly INavigationService _navigationService;

        public CommandsFactory([NotNull] INavigationService navigationService)
        {
            if (navigationService == null) throw new ArgumentNullException("navigationService");
            _navigationService = navigationService;
        }

        public ICommand GetShowTeachersTimeTableCommand()
        {
            return new ShowTeachersTimeTableCommand(_navigationService);
        }
    }
}
