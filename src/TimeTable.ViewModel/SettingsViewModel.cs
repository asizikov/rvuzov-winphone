using System;
using JetBrains.Annotations;
using TimeTable.ViewModel.Services;


namespace TimeTable.ViewModel
{
    public sealed class SettingsViewModel : BaseViewModel
    {
        private readonly BaseApplicationSettings _applicationSettings;
        private readonly FlurryPublisher _flurryPublisher;

        public SettingsViewModel([NotNull] BaseApplicationSettings applicationSettings, [NotNull] INavigationService navigationService,
            [NotNull] FlurryPublisher flurryPublisher)
        {
            if (applicationSettings == null) throw new ArgumentNullException("applicationSettings");
            if (navigationService == null) throw new ArgumentNullException("navigationService");
            if (flurryPublisher == null) throw new ArgumentNullException("flurryPublisher");
            _applicationSettings = applicationSettings;
            _flurryPublisher = flurryPublisher;
            _flurryPublisher.PublishPageLoadedSettings();
            Default = new DefaultViewModel(_applicationSettings.Me, navigationService);
        }

        [UsedImplicitly(ImplicitUseKindFlags.Access)]
        public DefaultViewModel Default { get; private set; }

    }
}
