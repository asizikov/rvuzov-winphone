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
using TimeTable.ViewModel.Utils;

namespace TimeTable.ViewModel
{
    public class GroupPageViewModel : BaseViewModel
    {
        private readonly INavigationService _navigation;
        private readonly BaseApplicationSettings _applicationSettings;
        private readonly AsyncDataProvider _dataProvider;
        private readonly int _universityId;
        private ReadOnlyObservableCollection<ListGroup<Group>> _groupsList;
        private Group _selectedGroup;
        private ICommand _showSearchBoxCommand;
        private bool _isSearchBoxVisible;
        private string _query;
        private IDisposable _queryObserver;
        private Groups _storedRequest;

        public GroupPageViewModel([NotNull] INavigationService navigation,
                                  [NotNull] BaseApplicationSettings applicationSettings,
                                  [NotNull] AsyncDataProvider dataProvider,
                                  int universityId)
        {
            if (dataProvider == null) throw new ArgumentNullException("dataProvider");
            if (navigation == null) throw new ArgumentNullException("navigation");
            if (applicationSettings == null) throw new ArgumentNullException("applicationSettings");

            _navigation = navigation;
            _applicationSettings = applicationSettings;
            _dataProvider = dataProvider;
            _universityId = universityId;

            _applicationSettings.UniversityId = _universityId;
            _showSearchBoxCommand = new SimpleCommand(() =>
            {
                IsSearchBoxVisible = true;
            });
            SubscribeToQuery();
            Init();
        }

        [UsedImplicitly(ImplicitUseKindFlags.Access)]
        public ReadOnlyObservableCollection<ListGroup<Group>> GroupsList
        {
            get { return _groupsList; }
            private set
            {
                if (Equals(value, _groupsList)) return;
                _groupsList = value;
                OnPropertyChanged("GroupsList");
            }
        }

        [UsedImplicitly(ImplicitUseKindFlags.Default)]
        public Group SelectedGroup
        {
            get { return _selectedGroup; }
            set
            {
                if (Equals(value, _selectedGroup)) return;
                _selectedGroup = value;
                OnPropertyChanged("SelectedGroup");
                NavigateToLessonsPage(_selectedGroup);
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

        private void NavigateToLessonsPage(Group group)
        {
            _applicationSettings.GroupId = group.Id;
            _applicationSettings.GroupName = group.GroupName;
            _navigation.GoToPage(Pages.Lessons, GetNavitationParameters(group));
        }

        private static IEnumerable<NavigationParameter> GetNavitationParameters(Group group)
        {
            return new List<NavigationParameter>{
                new NavigationParameter
                {
                    Parameter = NavigationParameterName.Id,
                    Value = group.Id.ToString(CultureInfo.InvariantCulture)
                },
                new NavigationParameter
                {
                    Parameter = NavigationParameterName.Name,
                    Value = group.GroupName
                }};
        }

        private void Init()
        {
            IsLoading = true;
            _dataProvider.GetUniversitesGroupsAsync(_universityId).Subscribe(
                result =>
                {
                    _storedRequest = result;
                    GroupsList = FormatResult(result.GroupsList);
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

        private static ReadOnlyObservableCollection<ListGroup<Group>> FormatResult(IEnumerable<Group> result)
        {
            var grouped = result
                .GroupBy(u => u.GroupName[0])
                .Select(g => new ListGroup<Group>(g.Key.ToString(CultureInfo.InvariantCulture),
                    g.ToList()));
           return new ReadOnlyObservableCollection<ListGroup<Group>>(
                        new ObservableCollection<ListGroup<Group>>(grouped));
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
            GroupsList = FormatResult(
                String.IsNullOrEmpty(search)
                    ? _storedRequest.GroupsList
                    : _storedRequest.GroupsList.Where(u => u.GroupName.IgnoreCaseContains(search)));
        }
    }
}