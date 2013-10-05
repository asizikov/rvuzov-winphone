using System;
using System.Windows.Input;
using JetBrains.Annotations;
using TimeTable.Model;
using TimeTable.ViewModel.Commands;
using TimeTable.ViewModel.Data;
using TimeTable.ViewModel.Services;

namespace TimeTable.ViewModel
{
    public class LessonsViewModel : BaseViewModel
    {
        private readonly INavigationService _navigation;
        private readonly BaseApplicationSettings _applicationSettings;
        private readonly ICommandFactory _commandFactory;
        private readonly AsyncDataProvider _dataProvider;
        private readonly bool _isTeacher;
        private readonly Group _group;
        private WeekViewModel _currentWeek;
        private WeekViewModel _nextWeek;
        private WeekViewModel _previousWeek;
        private University _university;

        public LessonsViewModel([NotNull] INavigationService navigation,
            [NotNull] BaseApplicationSettings applicationSettings, [NotNull] ICommandFactory commandFactory,
            [NotNull] AsyncDataProvider dataProvider, int id, bool isTeacher, int universityId)
        {
            if (navigation == null) throw new ArgumentNullException("navigation");
            if (applicationSettings == null) throw new ArgumentNullException("applicationSettings");
            if (commandFactory == null) throw new ArgumentNullException("commandFactory");
            if (dataProvider == null) throw new ArgumentNullException("dataProvider");
            _navigation = navigation;
            _applicationSettings = applicationSettings;
            _commandFactory = commandFactory;
            _dataProvider = dataProvider;
            _isTeacher = isTeacher;
            _group = new Group {GroupName = "", Id = id};

            if (_applicationSettings.GroupId != null)
            {
                _applicationSettings.GroupId = _group.Id;
            }


            Init(universityId);
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
            IsLoading = false;
            CurrentWeek = new WeekViewModel(timeTable.Data.Days, timeTable.Data.ParityCountdown, _commandFactory,
                WeekType.Current, _university);
            NextWeek = new WeekViewModel(timeTable.Data.Days, timeTable.Data.ParityCountdown, _commandFactory,
                WeekType.Next, _university);
            PreviousWeek = new WeekViewModel(timeTable.Data.Days, timeTable.Data.ParityCountdown, _commandFactory,
                WeekType.Previous, _university);
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
    }
}