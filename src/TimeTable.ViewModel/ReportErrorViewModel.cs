using System;
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
        private readonly FlurryPublisher _flurryPublisher;
        private readonly int _id;
        private readonly int _lessonId;
        private readonly bool _isTeacher;
        private readonly AsyncWebClient _webClient;
        private readonly INotificationService _notificationService;
        private readonly IUiStringsProviders _stringsProviders;
        private string _errorText;
        private SimpleCommand _sendErrorTextCommand;
        private bool _isInProgress;

        public ReportErrorViewModel([NotNull] INavigationService navigationService,
            [NotNull] FlurryPublisher flurryPublisher, int id, int lessonId, bool isTeacher,
            AsyncWebClient webClient, [NotNull] INotificationService notificationService,
            [NotNull] IUiStringsProviders stringsProviders)
        {
            if (navigationService == null) throw new ArgumentNullException("navigationService");
            if (flurryPublisher == null) throw new ArgumentNullException("flurryPublisher");
            if (notificationService == null) throw new ArgumentNullException("notificationService");
            if (stringsProviders == null) throw new ArgumentNullException("stringsProviders");
            _navigationService = navigationService;
            _flurryPublisher = flurryPublisher;
            _id = id;
            _lessonId = lessonId;
            _isTeacher = isTeacher;
            _webClient = webClient;
            _notificationService = notificationService;
            _stringsProviders = stringsProviders;
            _flurryPublisher.PublishPageLoadedReportError();
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

        [UsedImplicitly(ImplicitUseKindFlags.Access)]
        public bool IsInProgress
        {
            get { return _isInProgress; }
            private set
            {
                if (value.Equals(_isInProgress)) return;
                _isInProgress = value;
                OnPropertyChanged("IsInProgress");
            }
        }

        private void SendError()
        {
            if (IsInProgress) return;
            IsInProgress = true;
            _webClient.PostErrorMessageAsync(_id, _lessonId, _isTeacher, _errorText).Subscribe(result =>
            {
                if (result != null && result.Success)
                {
                    IsInProgress = false;
                    _notificationService.ShowToast(_stringsProviders.ThankYou, _stringsProviders.ReportErrorOk);
                    GoBack();
                }
                else
                {
                    StopProgressAndReportError();
                }
            }, ex => StopProgressAndReportError());
            _flurryPublisher.PublishReportError(_errorText);
        }

        private void StopProgressAndReportError()
        {
            IsInProgress = false;
            _notificationService.ShowToast(_stringsProviders.Oops, _stringsProviders.SomethingWentWrong);
        }

        private void GoBack()
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
}