using System;
using System.Windows.Input;
using JetBrains.Annotations;
using TimeTable.ViewModel.Commands;
using TimeTable.ViewModel.Services;

namespace TimeTable.ViewModel
{
    public sealed class ReportErrorViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private string _errorText;
        private ICommand _sendErrorTextCommand;

        public ReportErrorViewModel([NotNull] INavigationService navigationService)
        {
            if (navigationService == null) throw new ArgumentNullException("navigationService");
            _navigationService = navigationService;
            SendErrorTextCommand = new SimpleCommand(SendError);
        }

        [UsedImplicitly(ImplicitUseKindFlags.Default)]
        public string ErrorText
        {
            get { return _errorText; }
            set
            {
                if (value == _errorText) return;
                _errorText = value;
                OnPropertyChanged("ErrorText");
            }
        }

        [UsedImplicitly(ImplicitUseKindFlags.Access)]
        public ICommand SendErrorTextCommand
        {
            get { return _sendErrorTextCommand; }
            private set
            {
                if (Equals(value, _sendErrorTextCommand)) return;
                _sendErrorTextCommand = value;
                OnPropertyChanged("SendErrorTextCommand");
            }
        }

        private void SendError()
        {
            if (_navigationService.CanGoBack())
            {
                _navigationService.GoBack();
            }
        }
    }
}