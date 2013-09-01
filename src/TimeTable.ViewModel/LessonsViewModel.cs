using System;
using JetBrains.Annotations;
using TimeTable.Model;
using TimeTable.ViewModel.Data;
using TimeTable.ViewModel.Services;

namespace TimeTable.ViewModel
{
    public class LessonsViewModel : BaseViewModel
    {
        private readonly INavigationService _navigation;
        private readonly BaseApplicationSettings _applicationSettings;
        private readonly AsyncDataProvider _dataProvider;
        private readonly Group _group;
        private WeekViewModel _currentWeek;
        private WeekViewModel _nextWeek;
        private WeekViewModel _previousWeek;

        public LessonsViewModel([NotNull] INavigationService navigation,
            [NotNull] BaseApplicationSettings applicationSettings,
            [NotNull] AsyncDataProvider dataProvider, int groupId, string groupName)
        {
            if (navigation == null) throw new ArgumentNullException("navigation");
            if (applicationSettings == null) throw new ArgumentNullException("applicationSettings");
            if (dataProvider == null) throw new ArgumentNullException("dataProvider");
            _navigation = navigation;
            _applicationSettings = applicationSettings;
            _dataProvider = dataProvider;
            _group = new Group { GroupName = groupName, Id = groupId };

            _applicationSettings.GroupId = _group.Id;

            Init();
        }

        private void Init()
        {
            IsLoading = true;
            _dataProvider.GetLessonsForGroupAsync(_group.Id).Subscribe(
                timeTable =>
                {
                    IsLoading = false;
                    CurrentWeek = new WeekViewModel(timeTable.Days, WeekType.Current);
                    NextWeek = new WeekViewModel(timeTable.Days, WeekType.Next);
                    PreviousWeek = new WeekViewModel(timeTable.Days, WeekType.Previous);
                },
            ex =>
            {
                IsLoading = false;
            }
            );
        }

        [UsedImplicitly(ImplicitUseKindFlags.Access)]
        public WeekViewModel PreviousWeek
        {
            get { return _previousWeek; }
            private set
            {
                if (Equals(value, _previousWeek)) return;
                _previousWeek = value;
                OnPropertyChanged("PreviousWeek");
            }
        }

        [UsedImplicitly(ImplicitUseKindFlags.Access)]
        public WeekViewModel CurrentWeek
        {
            get { return _currentWeek; }
            private set
            {
                if (Equals(value, _currentWeek)) return;
                _currentWeek = value;
                OnPropertyChanged("CurrentWeek");
            }
        }

        [UsedImplicitly(ImplicitUseKindFlags.Access)]
        public WeekViewModel NextWeek
        {
            get { return _nextWeek; }
            private set
            {
                if (Equals(value, _nextWeek)) return;
                _nextWeek = value;
                OnPropertyChanged("NextWeek");
            }
        }

        [UsedImplicitly(ImplicitUseKindFlags.Access)]
        public String GroupName
        {
            get { return _group.GroupName; }
        }
    }
}
