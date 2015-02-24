﻿using System;
using System.Windows.Input;
using JetBrains.Annotations;
using TimeTable.Domain;
using TimeTable.Domain.Internal;
using TimeTable.ViewModel.Commands;
using TimeTable.ViewModel.Services;

namespace TimeTable.ViewModel.ApplicationLevel
{
    public class FirstPageViewModel : BaseViewModel
    {
        private readonly INavigationService _navigation;
        private readonly BaseApplicationSettings _applicationSettings;
        private readonly IAsyncDataProvider _asyncDataProvider;
        private readonly FlurryPublisher _flurryPublisher;

        public FirstPageViewModel([NotNull] INavigationService navigation, [NotNull] BaseApplicationSettings applicationSettings, [NotNull] IAsyncDataProvider asyncDataProvider,
            [NotNull] FlurryPublisher flurryPublisher)
        {
            if (navigation == null) throw new ArgumentNullException("navigation");
            if (applicationSettings == null) throw new ArgumentNullException("applicationSettings");
            if (asyncDataProvider == null) throw new ArgumentNullException("asyncDataProvider");
            if (flurryPublisher == null) throw new ArgumentNullException("flurryPublisher");

            _navigation = navigation;
            _applicationSettings = applicationSettings;
            _asyncDataProvider = asyncDataProvider;
            _flurryPublisher = flurryPublisher;
            _flurryPublisher.PublishPageLoadedSelectRole();

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