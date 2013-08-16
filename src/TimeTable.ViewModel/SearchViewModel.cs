using System;
using System.ComponentModel;
using System.Reactive.Linq;
using System.Windows.Input;
using TimeTable.ViewModel.Commands;

namespace TimeTable.ViewModel
{
    public abstract class SearchViewModel : BaseViewModel
    {
        private IDisposable _queryObserver;
        private readonly ICommand _showSearchBoxCommand;
        private bool _isSearchBoxVisible;
        private string _query;

        protected SearchViewModel()
        {
            _showSearchBoxCommand = new SimpleCommand(() =>
            {
                IsSearchBoxVisible = true;
            });
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

        public ICommand ShowSearchBoxCommand
        {
            get
            {
                return _showSearchBoxCommand;
            }
        }

        public bool IsSearchBoxVisible
        {
            get { return _isSearchBoxVisible; }
            private set
            {
                if (value.Equals(_isSearchBoxVisible)) return;
                _isSearchBoxVisible = value;
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

        protected abstract void GetResults(string result);
    }
}