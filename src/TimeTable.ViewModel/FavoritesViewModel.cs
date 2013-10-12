using System;
using JetBrains.Annotations;
using TimeTable.ViewModel.Services;

namespace TimeTable.ViewModel
{
    public sealed class FavoritesViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly BaseApplicationSettings _applicationSettings;

        public FavoritesViewModel([NotNull] INavigationService navigationService,
            [NotNull] BaseApplicationSettings applicationSettings)
        {
            if (navigationService == null) throw new ArgumentNullException("navigationService");
            if (applicationSettings == null) throw new ArgumentNullException("applicationSettings");
            _navigationService = navigationService;
            _applicationSettings = applicationSettings;
        }
    }
}