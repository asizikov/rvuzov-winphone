using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Input;
using JetBrains.Annotations;
using Microsoft.Phone.Tasks;
using TimeTable.ViewModel.Commands;
using TimeTable.ViewModel.Services;

namespace TimeTable.ViewModel
{
    public abstract class SearchViewModel : BaseViewModel
    {
        private const string URL = "http://raspisaniye-vuzov.ru/webform/step1.html";

        protected readonly FlurryPublisher FlurryPublisher;
        private IDisposable _queryObserver;
        private readonly ICommand _showSearchBoxCommand;
        private readonly ICommand _notFoundCommand;
        private bool _isSearchBoxVisible;
        private string _query;

        protected SearchViewModel([NotNull] FlurryPublisher flurryPublisher)
        {
            if (flurryPublisher == null) throw new ArgumentNullException("flurryPublisher");
            FlurryPublisher = flurryPublisher;
            _showSearchBoxCommand = new SimpleCommand(() => { IsSearchBoxVisible = true; });
            _notFoundCommand = new SimpleCommand(NotFound);
        }

        public string Query
        {
            get { return _query; }
            set
            {
                if (value == _query) return;
                _query = value;
                OnPropertyChanged("Query");
            }
        }

        public Action<bool> OnLock { get; set; }

        public ICommand ShowSearchBoxCommand
        {
            get { return _showSearchBoxCommand; }
        }

        public ICommand OnFoundCommand
        {
            get { return _notFoundCommand; }
        }

        [UsedImplicitly(ImplicitUseKindFlags.Assign)]
        public bool IsSearchBoxVisible
        {
            get { return _isSearchBoxVisible; }
            set
            {
                if (value.Equals(_isSearchBoxVisible)) return;
                _isSearchBoxVisible = value;
                if (OnLock != null)
                {
                    OnLock(value);
                }
                OnPropertyChanged("IsSearchBoxVisible");
            }
        }

        public void ResetSearchState()
        {
            GetResults(String.Empty);
            IsSearchBoxVisible = false;
        }

        protected void SubscribeToQuery()
        {
            _queryObserver = (from evt in Observable.FromEventPattern<PropertyChangedEventArgs>(this, "PropertyChanged")
                where evt.EventArgs.PropertyName == "Query"
                select Query)
                .Throttle(TimeSpan.FromMilliseconds(500))
                .DistinctUntilChanged()
                .Subscribe(GetResults);
        }

        protected static ObservableCollection<ListGroup<T>> FormatResult<T>([NotNull] IEnumerable<T> result,
            Func<T, char> groupFunc)
        {
            var grouped = result
                .GroupBy(groupFunc)
                .Select(g => new ListGroup<T>(g.Key.ToString(CultureInfo.InvariantCulture),
                    g.ToList()));
            return (new ObservableCollection<ListGroup<T>>(grouped));
        }

        protected abstract void GetResults(string result);

        private void NotFound()
        {
            FlurryPublisher.PublishTimtableNotFoundEvent();
            var webBrowserTask = new WebBrowserTask { Uri = new Uri(URL) };
            webBrowserTask.Show();
        }
    }
}