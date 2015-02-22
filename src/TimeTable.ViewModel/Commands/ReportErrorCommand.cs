using System;
using System.Globalization;
using JetBrains.Annotations;
using TimeTable.ViewModel.Services;

namespace TimeTable.ViewModel.Commands
{
    public sealed class ReportErrorCommand : ITitledCommand
    {
        private readonly INavigationService _navigationService;
        private readonly FlurryPublisher _flurryPublisher;
        private readonly IUiStringsProviders _stringsProviders;
        private readonly int _lessonId;
        private readonly bool _isTeacher;
        private readonly int _holderId;

        public ReportErrorCommand([NotNull] INavigationService navigationService,
            [NotNull] FlurryPublisher flurryPublisher, [NotNull] IUiStringsProviders stringsProviders, int lessonId,
            bool isTeacher, int holderId)
        {
            if (navigationService == null) throw new ArgumentNullException("navigationService");
            if (flurryPublisher == null) throw new ArgumentNullException("flurryPublisher");
            if (stringsProviders == null) throw new ArgumentNullException("stringsProviders");
            _navigationService = navigationService;
            _flurryPublisher = flurryPublisher;
            _stringsProviders = stringsProviders;
            _lessonId = lessonId;
            _isTeacher = isTeacher;
            _holderId = holderId;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _navigationService.GoToPage(Pages.ReportErrorPage, new[]
            {
                new NavigationParameter
                {
                    Parameter = NavigationParameterName.Id,
                    Value = _holderId.ToString(CultureInfo.InvariantCulture)
                },
                new NavigationParameter
                {
                    Parameter = NavigationParameterName.LessonId,
                    Value = _lessonId.ToString(CultureInfo.InvariantCulture)
                },
                new NavigationParameter
                {
                    Parameter = NavigationParameterName.IsTeacher,
                    Value = _isTeacher.ToString()
                }
            });
        }

        public event EventHandler CanExecuteChanged;

        public string Title
        {
            get { return _stringsProviders.ReportError; }
        }
    }
}