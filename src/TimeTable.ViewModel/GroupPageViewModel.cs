using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using JetBrains.Annotations;
using TimeTable.Model;
using TimeTable.ViewModel.Data;
using TimeTable.ViewModel.Services;
using TimeTable.ViewModel.Utils;

namespace TimeTable.ViewModel
{
    public class GroupPageViewModel : SearchViewModel
    {
        private readonly INavigationService _navigation;
        private readonly BaseApplicationSettings _applicationSettings;
        private readonly AsyncDataProvider _dataProvider;
        private readonly FlurryPublisher _flurryPublisher;
        private readonly int _universityId;
        private ReadOnlyObservableCollection<ListGroup<Group>> _groupsList;
        private ReadOnlyObservableCollection<ListGroup<Teacher>> _teachersList;
        private Group _selectedGroup;
        private Groups _storedGroupsRequest;
        private Teachers _storedTeachersRequest;

        public GroupPageViewModel([NotNull] INavigationService navigation,
            [NotNull] BaseApplicationSettings applicationSettings, [NotNull] AsyncDataProvider dataProvider,
            [NotNull] FlurryPublisher flurryPublisher, int universityId)
        {
            if (dataProvider == null) throw new ArgumentNullException("dataProvider");
            if (flurryPublisher == null) throw new ArgumentNullException("flurryPublisher");
            if (navigation == null) throw new ArgumentNullException("navigation");
            if (applicationSettings == null) throw new ArgumentNullException("applicationSettings");

            _navigation = navigation;
            _applicationSettings = applicationSettings;
            _dataProvider = dataProvider;
            _flurryPublisher = flurryPublisher;
            _universityId = universityId;

            _applicationSettings.UniversityId = _universityId;
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

        [UsedImplicitly(ImplicitUseKindFlags.Access)]
        public ReadOnlyObservableCollection<ListGroup<Teacher>> TeachersList
        {
            get { return _teachersList; }
            private set
            {
                if (Equals(value, _teachersList)) return;
                _teachersList = value;
                OnPropertyChanged("TeachersList");
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
                if (_selectedGroup != null)
                {
                    _dataProvider.GetUniversityByIdAsync(_universityId)
                        .Subscribe(university =>
                        {
                            _flurryPublisher.PublishGroupSelected(_selectedGroup, university);
                            NavigateToLessonsPage(_selectedGroup);
                        });
                }
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
            return new List<NavigationParameter>
            {
                new NavigationParameter
                {
                    Parameter = NavigationParameterName.Id,
                    Value = group.Id.ToString(CultureInfo.InvariantCulture)
                },
                new NavigationParameter
                {
                    Parameter = NavigationParameterName.Name,
                    Value = group.GroupName
                }
            };
        }

        private void Init()
        {
            IsLoading = true;
            _dataProvider.GetUniversitesGroupsAsync(_universityId).Subscribe(
                result =>
                {
                    _storedGroupsRequest = result;
                    GroupsList = FormatResult(result.GroupsList, u => u.GroupName[0]);
                    IsLoading = false;
                },
                ex =>
                {
                    IsLoading = false;
                }
                );

            _dataProvider.GetUniversityTeachersAsync(_universityId).Subscribe(
                result =>
                {
                    _storedTeachersRequest = result;
                    TeachersList = FormatResult(result.TeachersList, u => u.Name[0]);
                }, ex => { IsLoading = false; }
                );
        }

        private static ReadOnlyObservableCollection<ListGroup<T>> FormatResult<T>([NotNull]IEnumerable<T> result,
            Func<T, char> groupFunc)
        {
            var grouped = result
                .GroupBy(groupFunc)
                .Select(g => new ListGroup<T>(g.Key.ToString(CultureInfo.InvariantCulture),
                    g.ToList()));
            return new ReadOnlyObservableCollection<ListGroup<T>>(
                new ObservableCollection<ListGroup<T>>(grouped));
        }

        protected override void GetResults(string search)
        {
            GroupsList = FormatResult(
                String.IsNullOrEmpty(search)
                    ? _storedGroupsRequest.GroupsList
                    : _storedGroupsRequest.GroupsList.Where(u => u.GroupName.IgnoreCaseContains(search)),
                u => u.GroupName[0]);
        }
    }
}