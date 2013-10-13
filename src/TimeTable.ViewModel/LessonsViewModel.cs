﻿using System;
using System.Windows.Input;
using JetBrains.Annotations;
using TimeTable.Model;
using TimeTable.ViewModel.Commands;
using TimeTable.ViewModel.Data;
using TimeTable.ViewModel.Services;
using TimeTable.ViewModel.Utils;

namespace TimeTable.ViewModel
{
    public class LessonsViewModel : BaseViewModel
    {
        private readonly INavigationService _navigation;
        private readonly BaseApplicationSettings _applicationSettings;
        private readonly ICommandFactory _commandFactory;
        private readonly AsyncDataProvider _dataProvider;
        private readonly FlurryPublisher _flurryPublisher;
        private readonly bool _isTeacher;
        private readonly Group _group;
        private int _selectedWeekIndex = 0;
        private WeekViewModel _currentWeek;
        private WeekViewModel _nextWeek;
        private WeekViewModel _previousWeek;
        private University _university;
        
        public LessonsViewModel([NotNull] INavigationService navigation, [NotNull] FlurryPublisher flurryPublisher,
            [NotNull] BaseApplicationSettings applicationSettings, [NotNull] ICommandFactory commandFactory,
            [NotNull] AsyncDataProvider dataProvider, int id, bool isTeacher, int universityId)
        {
            if (navigation == null) throw new ArgumentNullException("navigation");
            if (applicationSettings == null) throw new ArgumentNullException("applicationSettings");
            if (commandFactory == null) throw new ArgumentNullException("commandFactory");
            if (dataProvider == null) throw new ArgumentNullException("dataProvider");
            _navigation = navigation;
            _applicationSettings = applicationSettings;
            _flurryPublisher = flurryPublisher;
            _commandFactory = commandFactory;
            _dataProvider = dataProvider;
            _isTeacher = isTeacher;
            _group = new Group {GroupName = "", Id = id};

            Init(universityId);

            InitCommands();
        }

        private void Init(int universityId)
        {
            IsLoading = true;
            _dataProvider.GetUniversityByIdAsync(universityId).Subscribe(university =>
            {
                _university = university;
                LoadLessons();
            });
        }

        private void LoadLessons()
        {
            if (_isTeacher)
            {
                _dataProvider.GetLessonsForTeacherAsync(_group.Id)
                    .Subscribe(FormatTimeTable, ex => { IsLoading = false; });
            }
            else
            {
                _dataProvider.GetLessonsForGroupAsync(_group.Id).Subscribe(
                    FormatTimeTable,
                    ex => { IsLoading = false; }
                    );
            }
        }

        private void FormatTimeTable(Model.TimeTable timeTable)
        {
            var weekNumber = GetWeekNumber(timeTable.Data.ParityCountdown);

            CurrentWeek = new WeekViewModel(timeTable.Data.Days, weekNumber, _commandFactory,
                WeekType.Current, _university);
            NextWeek = new WeekViewModel(timeTable.Data.Days, weekNumber + 1, _commandFactory,
                WeekType.Next, _university);
            PreviousWeek = new WeekViewModel(timeTable.Data.Days, weekNumber - 1, _commandFactory,
                WeekType.Previous, _university);
            IsLoading = false;
        }

        private static int GetWeekNumber(long parityCountdown)
        {
            var parityCountDown = DateTimeUtils.DateTimeFromUnixTimestampSeconds(parityCountdown);

            var currentWeekNumber = DateTimeUtils.GetWeekNumber(DateTime.UtcNow);
            var firstWeekNumber = DateTimeUtils.GetWeekNumber(parityCountDown);

            if (currentWeekNumber > firstWeekNumber)
            {
                return currentWeekNumber - firstWeekNumber + 1;
            }
            return currentWeekNumber + (53 - firstWeekNumber) + 1; //todo: fixme
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

        public ICommand GoToSettingsCommand { get; private set; }
        public ICommand GoToFavoritesListCommand { get; private set; }
        public ICommand GoToTodayCommand { get; private set; }

        private void InitCommands()
        {
            GoToSettingsCommand = new SimpleCommand(NavigateToSettingsPage);
            GoToFavoritesListCommand = new SimpleCommand(NavigateToFavoritesPage);
            GoToTodayCommand = new SimpleCommand(SelectTodayItem);
        }

        
        private void NavigateToFavoritesPage()
        {
            _navigation.GoToPage(Pages.FarovitesPage);
        }

        private void NavigateToSettingsPage()
        {
            _flurryPublisher.PublishActionbarScheduleSettings(_university, _isTeacher, _group.GroupName, _group.Id);
            _navigation.GoToPage(Pages.SettingsPage);
        }

        public int SelectedWeekIndex
        {
            get { return _selectedWeekIndex; }
            
            set
            { if (Equals(value, _selectedWeekIndex)) return;
            _selectedWeekIndex = value;
            OnPropertyChanged("SelectedWeekIndex");
            }
        }

        private void SelectTodayItem()
        {
            SelectedWeekIndex = 0;
            DateTime today = DateTime.Now;
            CurrentWeek.SelectedDayIndex = (int)today.DayOfWeek - 1;
            CurrentWeek.SelectedDayIndex = -1;
            _flurryPublisher.PublishActionbarToday(_university, _isTeacher, _group.GroupName, _group.Id);
        }

    }
}