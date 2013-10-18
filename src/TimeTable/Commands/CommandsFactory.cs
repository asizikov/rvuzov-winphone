using System;
using JetBrains.Annotations;
using TimeTable.Model;
using TimeTable.ViewModel.Commands;
using TimeTable.ViewModel.Data;
using TimeTable.ViewModel.Services;

namespace TimeTable.Commands
{
    public sealed class CommandsFactory : ICommandFactory
    {
        private readonly INavigationService _navigationService;
        private readonly FlurryPublisher _flurryPublisher;
        private readonly IUiStringsProviders _stringsProviders;
        private readonly AsyncDataProvider _dataProvider;

        public CommandsFactory([NotNull] INavigationService navigationService, [NotNull] FlurryPublisher flurryPublisher,
            [NotNull] IUiStringsProviders stringsProviders, [NotNull] AsyncDataProvider dataProvider)
        {
            if (navigationService == null) throw new ArgumentNullException("navigationService");
            if (flurryPublisher == null) throw new ArgumentNullException("flurryPublisher");
            if (stringsProviders == null) throw new ArgumentNullException("stringsProviders");
            if (dataProvider == null) throw new ArgumentNullException("dataProvider");
            _navigationService = navigationService;
            _flurryPublisher = flurryPublisher;
            _stringsProviders = stringsProviders;
            _dataProvider = dataProvider;
        }

        [NotNull]
        public ITitledCommand GetShowTeachersTimeTableCommand(University university, LessonTeacher teacher)
        {
            return new ShowTeachersTimeTableCommand(_navigationService, _flurryPublisher, _stringsProviders, university,
                teacher);
        }

        [NotNull]
        public ITitledCommand GetShowGroupTimeTableCommand(University university, LessonGroup group)
        {
            return new ShowGroupTimeTableCommand(_navigationService, _flurryPublisher, _dataProvider, _stringsProviders,
                university, group);
        }
    }
}