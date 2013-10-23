using System;
using JetBrains.Annotations;
using TimeTable.ViewModel.Services;

namespace TimeTable.ViewModel.Commands
{
    public sealed class ReportErrorCommand : ITitledCommand
    {
        private readonly INavigationService _navigationService;
        private readonly FlurryPublisher _flurryPublisher;
        private readonly IUiStringsProviders _stringsProviders;

        public ReportErrorCommand([NotNull] INavigationService navigationService,
            [NotNull] FlurryPublisher flurryPublisher, [NotNull] IUiStringsProviders stringsProviders)
        {
            if (navigationService == null) throw new ArgumentNullException("navigationService");
            if (flurryPublisher == null) throw new ArgumentNullException("flurryPublisher");
            if (stringsProviders == null) throw new ArgumentNullException("stringsProviders");
            _navigationService = navigationService;
            _flurryPublisher = flurryPublisher;
            _stringsProviders = stringsProviders;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _navigationService.GoToPage(Pages.ReportErrorPage);
        }

        public event EventHandler CanExecuteChanged;

        public string Title
        {
            get
            {
                return _stringsProviders.ReportError;
            }
        }
    }
}