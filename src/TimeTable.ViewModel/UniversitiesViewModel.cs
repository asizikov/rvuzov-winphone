using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Input;
using JetBrains.Annotations;
using TimeTable.Model;
using TimeTable.ViewModel.Commands;
using TimeTable.ViewModel.Data;
using TimeTable.ViewModel.Services;

namespace TimeTable.ViewModel
{
    public class UniversitiesViewModel : BaseViewModel
    {
        private readonly AsyncDataProvider _dataProvider;
        private readonly FlurryPublisher _flurry;
        private readonly INavigationService _navigation;
        private readonly BaseApplicationSettings _applicationSettings;
        private ObservableCollection<ListGroup<University>> _universitesList;
        private University _selectedUniversity;
        private string _query;
        private IDisposable _queryObserver;
        private Universities _storedRequest;
        private readonly CultureInfo _invariantCulture;
        private ICommand _showSearchBoxCommand;
        private bool _isSearchBoxVisible;

        public UniversitiesViewModel([NotNull] INavigationService navigation,
            [NotNull] BaseApplicationSettings applicationSettings,
            [NotNull] AsyncDataProvider dataProvider,
            [NotNull] FlurryPublisher flurry)
        {
            if (dataProvider == null) throw new ArgumentNullException("dataProvider");
            if (flurry == null) throw new ArgumentNullException("flurry");
            if (navigation == null) throw new ArgumentNullException("navigation");

            _dataProvider = dataProvider;
            _flurry = flurry;
            _navigation = navigation;
            _applicationSettings = applicationSettings;
            _invariantCulture = CultureInfo.InvariantCulture;
            _showSearchBoxCommand = new SimpleCommand(() =>
            {
                IsSearchBoxVisible = true;
            });
            Init();
            SubscribeToQuery();
        }


        private void Init()
        {
            IsLoading = true;
            _dataProvider.GetUniversitesAsync().Subscribe(
                result =>
                {
                    _storedRequest = result;
                    UniversitesList = FormatResult(result.UniversitesList);
                    IsLoading = false;
                },
                ex =>
                {
                    IsLoading = false;
                    //handle exception
                },
                () =>
                {
                    //handle loaded
                }
                );
        }

        private static ObservableCollection<ListGroup<University>> FormatResult(IEnumerable<University> result)
        {
            var grouped = result
                .GroupBy(u => u.ShortName[0])
                .Select(g => new ListGroup<University>(g.Key.ToString(CultureInfo.InvariantCulture),
                    g.ToList()));
            var observableCollection = new ObservableCollection<ListGroup<University>>(grouped);
            return observableCollection;
        }

        [UsedImplicitly(ImplicitUseKindFlags.Access)]
        public ObservableCollection<ListGroup<University>> UniversitesList
        {
            get { return _universitesList; }
            private set
            {
                if (Equals(value, _universitesList)) return;
                _universitesList = value;
                OnPropertyChanged("UniversitesList");
            }
        }

        [UsedImplicitly(ImplicitUseKindFlags.Default)]
        public University SelectedUniversity
        {
            get { return _selectedUniversity; }
            set
            {
                if (Equals(value, _selectedUniversity)) return;
                _selectedUniversity = value;
                OnPropertyChanged("SelectedUniversity");

                if (_selectedUniversity != null)
                {
                    _flurry.PublishUniversitySelected(_selectedUniversity);
                    NavigateToUniversity(_selectedUniversity.Id);
                }
            }
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
            set
            {
                if (value.Equals(_isSearchBoxVisible)) return;
                _isSearchBoxVisible = value;
                OnPropertyChanged("IsSearchBoxVisible");
            }
        }

        private void NavigateToUniversity(int id)
        {
            var navigationParameter = new NavigationParameter
            {
                Parameter = NavigationParameterName.Id,
                Value = id.ToString(_invariantCulture)
            };
            _navigation.GoToPage(Pages.Groups, new List<NavigationParameter> {navigationParameter});
        }

        private void SubscribeToQuery()
        {
            _queryObserver = (from evt in Observable.FromEventPattern<PropertyChangedEventArgs>(this, "PropertyChanged")
                where evt.EventArgs.PropertyName == "Query"
                select Query)
                .Throttle(TimeSpan.FromMilliseconds(500))
                .DistinctUntilChanged()
                .Subscribe(GetResults);
        }

        private void GetResults(string search)
        {
            UniversitesList = FormatResult(
                String.IsNullOrEmpty(search)
                    ? _storedRequest.UniversitesList
                    : _storedRequest.UniversitesList.Where(u => Matches(u, search)));
        }

        private bool Matches(University university, string search)
        {
            return IgnoreCaseContains(university.Name, search) ||
                   IgnoreCaseContains(university.ShortName, search);
        }

        private bool IgnoreCaseContains(string text, string search)
        {
            return _invariantCulture.CompareInfo.IndexOf(text, search, CompareOptions.IgnoreCase) >= 0;
        }
    }
}