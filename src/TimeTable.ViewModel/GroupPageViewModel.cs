using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using JetBrains.Annotations;
using TimeTable.Model;
using TimeTable.ViewModel.Data;
using TimeTable.ViewModel.Services;

namespace TimeTable.ViewModel
{
    public class GroupPageViewModel : BaseViewModel
    {
        private readonly INavigationService _navigation;
        private readonly BaseApplicationSettings _applicationSettings;
        private readonly AsyncDataProvider _dataProvider;
        private readonly int _universityId;
        private ReadOnlyObservableCollection<Group> _groupsList;
        private Group _selectedGroup;

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

            Init();
        }

        [UsedImplicitly(ImplicitUseKindFlags.Access)]
        public ReadOnlyObservableCollection<Group> GroupsList
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

        private void NavigateToLessonsPage(Group group)
        {
            _applicationSettings.GroupId = group.Id;
            _applicationSettings.GroupName = group.GroupName;
            _navigation.GoToPage(Pages.Lessons, new List<NavigationParameter>{
                new NavigationParameter
            {
                Parameter = NavigationParameterName.Id,
                Value = group.Id.ToString(CultureInfo.InvariantCulture)
            },
            new NavigationParameter
            {
                Parameter = NavigationParameterName.Name,
                Value = group.GroupName
            }});
        }

        private void Init()
        {
            IsLoading = true;
            _dataProvider.GetUniversitesGroupsAsync(_universityId).Subscribe(
                result =>
                {
                    IsLoading = false;
                    GroupsList = new ReadOnlyObservableCollection<Group>(
                        new ObservableCollection<Group>(result.GroupsList));
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
    }
}