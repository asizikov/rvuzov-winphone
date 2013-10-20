using System;
using JetBrains.Annotations;
using TimeTable.ViewModel.Services;


namespace TimeTable.ViewModel
{
    public sealed class SettingsViewModel : BaseViewModel
    {
        private readonly BaseApplicationSettings _applicationSettings;

        public SettingsViewModel([NotNull] BaseApplicationSettings applicationSettings,
            [NotNull] INavigationService navigationService)
        {
            if (applicationSettings == null) throw new ArgumentNullException("applicationSettings");
            if (navigationService == null) throw new ArgumentNullException("navigationService");
            _applicationSettings = applicationSettings;
            Default = new DefaultViewModel(_applicationSettings.Me, navigationService);
        }

        [UsedImplicitly(ImplicitUseKindFlags.Access)]
        public DefaultViewModel Default { get; private set; }

    }
}
