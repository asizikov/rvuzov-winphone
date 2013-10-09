﻿using System;
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
        private readonly IUiStringsProviders _stringsProviders;

        public CommandsFactory([NotNull] INavigationService navigationService, [NotNull] FlurryPublisher flurryPublisher,
            [NotNull] IUiStringsProviders stringsProviders)
        {
            if (navigationService == null) throw new ArgumentNullException("navigationService");
            if (flurryPublisher == null) throw new ArgumentNullException("flurryPublisher");
            if (stringsProviders == null) throw new ArgumentNullException("stringsProviders");
            _navigationService = navigationService;
            _flurryPublisher = flurryPublisher;
            _stringsProviders = stringsProviders;
        }

        public ITitledCommand GetShowTeachersTimeTableCommand(University university, LessonTeacher teacher)
        {
            return new ShowTeachersTimeTableCommand(_navigationService, _flurryPublisher, _stringsProviders, university,
                teacher);
        }
    }
}