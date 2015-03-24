using System;
using JetBrains.Annotations;
using TimeTable.Mvvm;
using TimeTable.Mvvm.Navigation;
using TimeTable.ViewModel.Services;

namespace TimeTable.ViewModel.ApplicationLevel
{
    public sealed class SettingsViewModel : PageViewModel
    {
        public SettingsViewModel([NotNull] BaseApplicationSettings applicationSettings,
                                 [NotNull] INavigationService navigationService,
                                 [NotNull] FlurryPublisher flurryPublisher)
        {
            if (applicationSettings == null) throw new ArgumentNullException("applicationSettings");
            if (navigationService == null) throw new ArgumentNullException("navigationService");
            if (flurryPublisher == null) throw new ArgumentNullException("flurryPublisher");
            flurryPublisher.PublishPageLoadedSettings();
            Default = new DefaultViewModel(applicationSettings.Me, navigationService);
        }

        [UsedImplicitly(ImplicitUseKindFlags.Access)]
        public DefaultViewModel Default { get; private set; }
    }
}