using System;
using JetBrains.Annotations;
using Microsoft.Phone.Tasks;
using TimeTable.ViewModel.Services;

namespace TimeTable.ViewModel.Commands
{
    public class UpdateLessonCommand : ITitledCommand
    {
        private const string URL = "http://raspisaniye-vuzov.ru/webform/step1.html";
        private readonly FlurryPublisher _flurryPublisher;
        private readonly IUiStringsProviders _stringsProviders;

        public UpdateLessonCommand([NotNull] FlurryPublisher flurryPublisher,
            [NotNull] IUiStringsProviders stringsProviders)
        {
            if (flurryPublisher == null) throw new ArgumentNullException("flurryPublisher");
            if (stringsProviders == null) throw new ArgumentNullException("stringsProviders");
            _flurryPublisher = flurryPublisher;
            _stringsProviders = stringsProviders;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _flurryPublisher.PublishUpdateLessonEvent();
            var webBrowserTask = new WebBrowserTask { Uri = new Uri(URL) };
            webBrowserTask.Show();
        }

        public event EventHandler CanExecuteChanged;

        public string Title
        {
            get { return _stringsProviders.UpdateLesson; }
        }
    }
}