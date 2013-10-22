using System;
using System.Windows.Input;
using System.Windows.Threading;
using JetBrains.Annotations;
using TimeTable.ViewModel.Commands;
using TimeTable.ViewModel.Data;
using TimeTable.ViewModel.Services;

namespace TimeTable.ViewModel
{
    public sealed class ReportErrorViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly int _id;
        private readonly int _lessonId;
        private readonly bool _isTeacher;
        private readonly AsyncWebClient _webClient;
        private string _errorText;
        private SimpleCommand _sendErrorTextCommand;

        public ReportErrorViewModel([NotNull] INavigationService navigationService, int id, int lessonId, bool isTeacher,
            AsyncWebClient webClient)
        {
            if (navigationService == null) throw new ArgumentNullException("navigationService");
            _navigationService = navigationService;
            _id = id;
            _lessonId = lessonId;
            _isTeacher = isTeacher;
            _webClient = webClient;
            SendErrorTextCommand = new SimpleCommand(SendError, () => !string.IsNullOrWhiteSpace(ErrorText));
        }

        [UsedImplicitly(ImplicitUseKindFlags.Default)]
        public string ErrorText
        {
            get { return _errorText; }
            set
            {
                if (value == _errorText) return;
                _errorText = value;
                SendErrorTextCommand.RaiseCanExecuteChanged();
                OnPropertyChanged("ErrorText");
            }
        }

        [UsedImplicitly(ImplicitUseKindFlags.Access)]
        public SimpleCommand SendErrorTextCommand
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
            _webClient.PostErrorMessageAsync(_id, _lessonId, _isTeacher, _errorText).Subscribe(result =>
            {
                if (result != null)
                {
                    if (result.Success)
                    {
                        SmartDispatcher.BeginInvoke(() =>
                        {
                            if (_navigationService.CanGoBack())
                            {
                                _navigationService.GoBack();
                            }
                        });
                    }
                }
            });
            
        }
    }
}