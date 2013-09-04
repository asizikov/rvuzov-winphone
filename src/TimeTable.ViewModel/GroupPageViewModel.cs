﻿using System;
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
        private int _selectedPivotIndex;
        private readonly Func<Group, char> _groupFunc;
        private readonly Func<Teacher, char> _teachersGroupFunc;

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

            _groupFunc = group => group.GroupName[0];
            _teachersGroupFunc = teacher => teacher.Name[0];

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

        [UsedImplicitly(ImplicitUseKindFlags.Assign)]
        public int SelectedPivotIndex
        {
            get { return _selectedPivotIndex; }
            set
            {
                if (value == _selectedPivotIndex) return;
                _selectedPivotIndex = value;
                OnPropertyChanged("SelectedPivotIndex");
            }
        }

        private void Init()
        {
            IsLoading = true;
            _dataProvider.GetUniversitesGroupsAsync(_universityId).Subscribe(
                result =>
                {
                    _storedGroupsRequest = result;
                    GroupsList = FormatResult(result.GroupsList, _groupFunc);
                    IsLoading = false;
                },
                ex => { IsLoading = false; }
                );

            _dataProvider.GetUniversityTeachersAsync(_universityId).Subscribe(
                result =>
                {
                    _storedTeachersRequest = result;
                    TeachersList = FormatResult(result.TeachersList, u => u.Name[0]);
                }, ex => { IsLoading = false; }
                );
        }

        private static ReadOnlyObservableCollection<ListGroup<T>> FormatResult<T>([NotNull] IEnumerable<T> result,
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
            if (String.IsNullOrEmpty(search))
            {
                GroupsList = FormatResult(_storedGroupsRequest.GroupsList, _groupFunc);
                TeachersList = FormatResult(_storedTeachersRequest.TeachersList, _teachersGroupFunc);
                return;
            }
            //TODO: use enum here.
            if (SelectedPivotIndex == 0) // Groups
            {
                GroupsList =
                    FormatResult(_storedGroupsRequest.GroupsList.Where(u => u.GroupName.IgnoreCaseContains(search)),
                        _groupFunc);
            }
            else if (SelectedPivotIndex == 1) // Teachers
            {
                TeachersList =
                    FormatResult(
                        _storedTeachersRequest.TeachersList.Where(teacher => teacher.Name.IgnoreCaseContains(search)),
                        _teachersGroupFunc);
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
                    Parameter = NavigationParameterName.IsTeacher,
                    Value = false.ToString()
                },
                new NavigationParameter
                {
                    Parameter = NavigationParameterName.Name,
                    Value = group.GroupName
                }
            };
        }
    }
}