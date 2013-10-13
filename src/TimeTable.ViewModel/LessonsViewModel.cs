using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using JetBrains.Annotations;
using TimeTable.Model;
using TimeTable.ViewModel.Commands;
using TimeTable.ViewModel.Data;
using TimeTable.ViewModel.Services;
using TimeTable.ViewModel.Utils;

namespace TimeTable.ViewModel
{
    public sealed class LessonsViewModel : BaseViewModel
    {
        private readonly INavigationService _navigation;
        private readonly BaseApplicationSettings _applicationSettings;
        private readonly ICommandFactory _commandFactory;
        private readonly AsyncDataProvider _dataProvider;
        private readonly FavoritedItemsManager _favoritedItemsManager;
        private readonly IUiStringsProviders _stringsProviders;
        private readonly int _id;
        private readonly FlurryPublisher _flurryPublisher;
        private readonly bool _isTeacher;
        private readonly int _facultyId;
        private WeekViewModel _currentWeek;
        private WeekViewModel _nextWeek;
        private WeekViewModel _previousWeek;
        private University _university;
        private Teacher _teacher;
        private Group _group;
        private FavoritedState _favoritedState;
        private ObservableCollection<AppbarButtonViewModel> _appbarButtons;
        private AppbarButtonViewModel _favoriteAppbarButton;

        public LessonsViewModel([NotNull] INavigationService navigation, [NotNull] FlurryPublisher flurryPublisher,
            [NotNull] BaseApplicationSettings applicationSettings, [NotNull] ICommandFactory commandFactory,
            [NotNull] AsyncDataProvider dataProvider, [NotNull] FavoritedItemsManager favoritedItemsManager,
            [NotNull] IUiStringsProviders stringsProviders ,int id,
            bool isTeacher, int universityId, int facultyId)
        {
            if (navigation == null) throw new ArgumentNullException("navigation");
            if (applicationSettings == null) throw new ArgumentNullException("applicationSettings");
            if (commandFactory == null) throw new ArgumentNullException("commandFactory");
            if (dataProvider == null) throw new ArgumentNullException("dataProvider");
            if (favoritedItemsManager == null) throw new ArgumentNullException("favoritedItemsManager");
            if (stringsProviders == null) throw new ArgumentNullException("stringsProviders");

            _navigation = navigation;
            _applicationSettings = applicationSettings;
            _flurryPublisher = flurryPublisher;
            _commandFactory = commandFactory;
            _dataProvider = dataProvider;
            _favoritedItemsManager = favoritedItemsManager;
            _stringsProviders = stringsProviders;
            _id = id;
            _isTeacher = isTeacher;
            _facultyId = facultyId;

            Init(universityId);

            InitCommands();
            BuildAppBarButtons();
            UpdateFaforitedSate();
        }

        private void BuildAppBarButtons()
        {
            AppbarButtons = new ObservableCollection<AppbarButtonViewModel>();
            AppbarButtons.Add(new AppbarButtonViewModel
            {
                Text = _stringsProviders.Settings,
                Command = GoToSettingsCommand,
                IconUri = "/Resources/Icons/appbar.cog.png"
            });
            _favoriteAppbarButton = new AppbarButtonViewModel
            {
                Text = _stringsProviders.AddToFavorited,
                Command = AddToFavoritesCommand,
                IconUri = "/Resources/Icons/favs.addto.png"
            };
            AppbarButtons.Add(_favoriteAppbarButton);
        }

        private void Init(int universityId)
        {
            IsLoading = true;
            _dataProvider.GetUniversityByIdAsync(universityId).Subscribe(university =>
            {
                _university = university;
                if (_isTeacher)
                {
                    _dataProvider.GetTeacherByIdAsync(universityId, _id).Subscribe(teacher =>
                    {
                        _teacher = teacher;
                        UpdateFaforitedSate();
                    });
                }
                else
                {
                    _dataProvider.GetGroupByIdAsync(_facultyId, _id).Subscribe(group =>
                    {
                        _group = group;
                        UpdateFaforitedSate();
                    });
                }
                LoadLessons();
            });
        }

        private void LoadLessons()
        {
            if (_isTeacher)
            {
                _dataProvider.GetLessonsForTeacherAsync(_id)
                    .Subscribe(FormatTimeTable, ex => { IsLoading = false; });
            }
            else
            {
                _dataProvider.GetLessonsForGroupAsync(_id).Subscribe(
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
        public string GroupName
        {
            get { return _group == null ? string.Empty : _group.GroupName; }
        }

        [UsedImplicitly(ImplicitUseKindFlags.Access)]
        public FavoritedState FavoritedState
        {
            get { return _favoritedState; }
            private set
            {
                if (value == _favoritedState) return;
                _favoritedState = value;
                OnPropertyChanged("FavoritedState");
            }
        }

        [UsedImplicitly(ImplicitUseKindFlags.Access)]
        public ICommand GoToSettingsCommand { get; private set; }

        [UsedImplicitly(ImplicitUseKindFlags.Access)]
        public ICommand GoToFavoritesListCommand { get; private set; }

        [UsedImplicitly(ImplicitUseKindFlags.Access)]
        public ICommand AddToFavoritesCommand { get; private set; }

        private void InitCommands()
        {
            GoToSettingsCommand = new SimpleCommand(NavigateToSettingsPage);
            GoToFavoritesListCommand = new SimpleCommand(NavigateToFavoritesPage);
            AddToFavoritesCommand = new SimpleCommand(AddToFavorites);
        }

        private void AddToFavorites()
        {
            _favoritedItemsManager.Add(_isTeacher, _isTeacher ? _teacher.Id : _group.Id,
                _isTeacher ? _teacher.Name : _group.GroupName, _university, _facultyId);
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

        private void UpdateFaforitedSate()
        {
            if (_group == null && _teacher == null)
            {
                FavoritedState = FavoritedState.Unknown;
                return;
            }
            if (_favoritedItemsManager.IsGroupFavorited(_facultyId, _group.Id))
            {
                FavoritedState = FavoritedState.Favorited;
                return;
            }
            FavoritedState = FavoritedState.NotFavorited;
        }

        [UsedImplicitly(ImplicitUseKindFlags.Access)]
        public ObservableCollection<AppbarButtonViewModel> AppbarButtons
        {
            get { return _appbarButtons; }
            set
            {
                if (Equals(value, _appbarButtons)) return;
                _appbarButtons = value;
                OnPropertyChanged("AppbarButtons");
            }
        }
    }
}