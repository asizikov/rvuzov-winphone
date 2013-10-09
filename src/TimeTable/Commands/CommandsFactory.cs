using System;
using System.Windows.Input;
using JetBrains.Annotations;
using TimeTable.Model;
using TimeTable.ViewModel.Commands;
using TimeTable.ViewModel.Services;

namespace TimeTable.Commands
{
    public sealed class CommandsFactory : ICommandFactory
    {
        private readonly INavigationService _navigationService;
        private readonly FlurryPublisher _flurryPublisher;

        public CommandsFactory([NotNull] INavigationService navigationService, [NotNull] FlurryPublisher flurryPublisher)
        {
            if (navigationService == null) throw new ArgumentNullException("navigationService");
            if (flurryPublisher == null) throw new ArgumentNullException("flurryPublisher");
            _navigationService = navigationService;
            _flurryPublisher = flurryPublisher;
        }

        public ICommand GetShowTeachersTimeTableCommand(University university, LessonTeacher teacher)
        {
            return new ShowTeachersTimeTableCommand(_navigationService, _flurryPublisher, university, teacher);
        }
    }
}
