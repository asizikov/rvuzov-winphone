using System;
using JetBrains.Annotations;
using Microsoft.Phone.Tasks;
using TimeTable.Domain.Participants;
using TimeTable.ViewModel.Commands;
using TimeTable.ViewModel.OrganizationalStructure;
using TimeTable.ViewModel.Services;

namespace TimeTable.ViewModel.WeekOverview.Commands
{
    public class UpdateLessonCommand : ITitledCommand
    {
        private const string URL = "http://raspisaniye-vuzov.ru/webform/step1.html";
        private readonly FlurryPublisher _flurryPublisher;
        private readonly IUiStringsProviders _stringsProviders;
        private readonly NavigationFlow _navigationFlow;
        [CanBeNull] private readonly Group _group;

        public UpdateLessonCommand([NotNull] FlurryPublisher flurryPublisher,
                                   [NotNull] IUiStringsProviders stringsProviders, NavigationFlow navigationFlow,
                                   [CanBeNull] Group group)
        {
            if (flurryPublisher == null) throw new ArgumentNullException("flurryPublisher");
            if (stringsProviders == null) throw new ArgumentNullException("stringsProviders");
            _flurryPublisher = flurryPublisher;
            _stringsProviders = stringsProviders;
            _navigationFlow = navigationFlow;
            _group = @group;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _flurryPublisher.PublishTimtableNotFoundEvent(_navigationFlow, _group);
            var webBrowserTask = new WebBrowserTask {Uri = new Uri(URL)};
            webBrowserTask.Show();
        }

        public event EventHandler CanExecuteChanged;

        public string Title
        {
            get { return _stringsProviders.UpdateLesson; }
        }
    }
}