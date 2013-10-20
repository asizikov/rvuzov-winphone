using System;
using JetBrains.Annotations;
using TimeTable.Model.Internal;
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

    public sealed class DefaultViewModel
    {
        private readonly Me _model;
        private readonly INavigationService _navigationService;

        public DefaultViewModel(Me model, INavigationService navigationService)
        {
            _model = model;
            _navigationService = navigationService;
        }

        public string Name
        {
            get
            {
                return _model.Teacher != null ? _model.Teacher.Name : _model.DefaultGroup.GroupName;
            }
        }

        public string University
        {
            get
            {
                return _model.University.Name;
            }
        }
    }
}
