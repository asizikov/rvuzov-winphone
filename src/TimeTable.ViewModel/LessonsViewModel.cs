using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Threading;
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
        private int _selectedWeekIndex;
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
        private AppbarButtonViewModel _unfavoriteAppbarButton;
        private string _title;

        public LessonsViewModel([NotNull] INavigationService navigation, [NotNull] FlurryPublisher flurryPublisher,
            [NotNull] BaseApplicationSettings applicationSettings, [NotNull] ICommandFactory commandFactory,
            [NotNull] AsyncDataProvider dataProvider, [NotNull] FavoritedItemsManager favoritedItemsManager,
            [NotNull] IUiStringsProviders stringsProviders, int id,
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
            AppbarButtons = new ObservableCollection<AppbarButtonViewModel>
            {
                new AppbarButtonViewModel
                {
                    Text = _stringsProviders.Settings,
                    Command = GoToSettingsCommand,
                    IconUri = "/Resources/Icons/appbar.cog.png"
                },

                new AppbarButtonViewModel
                {
                Text = _stringsProviders.Today,
                IconUri = "/Resources/Icons/feature.calendar.png",
                Command = GoToTodayCommand,
                }
            };
            _favoriteAppbarButton = new AppbarButtonViewModel
            {
                Text = _stringsProviders.AddToFavorited,
                Command = AddToFavoritesCommand,
                IconUri = "/Resources/Icons/favs.addto.png"
            };

            _unfavoriteAppbarButton = new AppbarButtonViewModel
            {
                Text = _stringsProviders.Unfavorite,
                IconUri = "/Resources/Icons/appbar.star.minus.png",
                Command = RemoveFromFavoritesCommand,
            };
            
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
                        Title = _teacher.Name;
                        UpdateFaforitedSate();
                    });
                }
                else
                {
                    _dataProvider.GetGroupByIdAsync(_facultyId, _id).Subscribe(group =>
                    {
                        _group = group;
                        Title = _group.GroupName;
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
        public string Title
        {
            get { return _title; }
            private set
            {
                if (value == _title) return;
                _title = value;
                OnPropertyChanged("Title");
            }
        }

        [UsedImplicitly(ImplicitUseKindFlags.Access)]
        public FavoritedState FavoritedState
        {
            get { return _favoritedState; }
            private set
            {
                if (value == _favoritedState) return;
                _favoritedState = value;
                UpdateAppBar();
                OnPropertyChanged("FavoritedState");
            }
        }

        private void UpdateAppBar()
        {
            SmartDispatcher.BeginInvoke(() =>
            {
                switch (FavoritedState)
                {
                    case FavoritedState.Unknown:
                        break;
                    case FavoritedState.Me:
                        AppbarButtons.Remove(_favoriteAppbarButton);
                        AppbarButtons.Remove(_unfavoriteAppbarButton);
                        break;
                    case FavoritedState.Favorited:
                        AppbarButtons.Remove(_favoriteAppbarButton);
                        AppbarButtons.Add(_unfavoriteAppbarButton);
                        break;
                    case FavoritedState.NotFavorited:
                        AppbarButtons.Remove(_unfavoriteAppbarButton);
                        AppbarButtons.Add(_favoriteAppbarButton);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            });
        }

        [UsedImplicitly(ImplicitUseKindFlags.Access)]
        public ICommand GoToSettingsCommand { get; private set; }

        [UsedImplicitly(ImplicitUseKindFlags.Access)]
        public ICommand GoToFavoritesListCommand { get; private set; }
        public ICommand GoToTodayCommand { get; private set; }

        [UsedImplicitly(ImplicitUseKindFlags.Access)]
        public ICommand AddToFavoritesCommand { get; private set; }

        [UsedImplicitly(ImplicitUseKindFlags.Access)]
        public ICommand RemoveFromFavoritesCommand { get; private set; }

        private void InitCommands()
        {
            GoToSettingsCommand = new SimpleCommand(NavigateToSettingsPage);
            GoToFavoritesListCommand = new SimpleCommand(NavigateToFavoritesPage);

            GoToTodayCommand = new SimpleCommand(SelectTodayItem);
            AddToFavoritesCommand = new SimpleCommand(AddToFavorites);
            RemoveFromFavoritesCommand = new SimpleCommand(RemoveFromFavorites);
        }

        private void RemoveFromFavorites()
        {
            _favoritedItemsManager.Remove(_isTeacher, _isTeacher ? _teacher.Id : _group.Id,
                _isTeacher ? _teacher.Name : _group.GroupName, _university, _facultyId);
            FavoritedState = FavoritedState.NotFavorited;
        }

        private void AddToFavorites()
        {
            _favoritedItemsManager.Add(_isTeacher, _isTeacher ? _teacher.Id : _group.Id,
                _isTeacher ? _teacher.Name : _group.GroupName, _university, _facultyId);
            FavoritedState = FavoritedState.Favorited;
            _flurryPublisher.PublishMarkFavorite(_university, _isTeacher,
                (_isTeacher ? _teacher.Name : _group.GroupName), (_isTeacher ? _teacher.Id : _group.Id));

        }

        
        private void NavigateToFavoritesPage()
        {
            _navigation.GoToPage(Pages.FarovitesPage);
        }

        private void NavigateToSettingsPage()
        {
            _flurryPublisher.PublishActionbarScheduleSettings(_university, _isTeacher,
                (_isTeacher ? _teacher.Name : _group.GroupName), (_isTeacher ? _teacher.Id : _group.Id));
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
            DateTime today = DateTime.UtcNow;
            int todayIndex = (int)today.DayOfWeek - 1;

            SelectedWeekIndex = 0;
            CurrentWeek.SelectedDayItem = CurrentWeek.Days[todayIndex];
            CurrentWeek.SelectedDayItem = null;
            _flurryPublisher.PublishActionbarToday(_university, _isTeacher, _group.GroupName, _group.Id);
        }

        private void UpdateFaforitedSate()
        {
            if (_group == null && _teacher == null)
            {
                FavoritedState = FavoritedState.Unknown;
                return;
            }
            if (!_isTeacher && _favoritedItemsManager.IsGroupFavorited(_facultyId, _group.Id))
            {
                FavoritedState = FavoritedState.Favorited;
                return;
            }
            if (_isTeacher && _favoritedItemsManager.IsTeacherFavorited(_university.Id, _teacher.Id))
            {
                FavoritedState = FavoritedState.Favorited;
                return;
            }
            if (!_isTeacher)
            {
                if (_applicationSettings.Me.DefaultGroup != null &&
                    _applicationSettings.Me.DefaultGroup.Id == _group.Id &&
                    _applicationSettings.Me.Faculty.Id == _facultyId)
                {
                    FavoritedState = FavoritedState.Me;
                    return;
                }
            }
            else
            {
                if (_applicationSettings.Me.Teacher != null &&
                    _applicationSettings.Me.Teacher.Id == _teacher.Id &&
                    _applicationSettings.Me.University.Id == _university.Id)
                {
                    FavoritedState = FavoritedState.Me;
                    return;
                }
            }
            FavoritedState = FavoritedState.NotFavorited;
        }

        [UsedImplicitly(ImplicitUseKindFlags.Access)]
        public ObservableCollection<AppbarButtonViewModel> AppbarButtons
        {
            get { return _appbarButtons; }
            private set
            {
                if (Equals(value, _appbarButtons)) return;
                _appbarButtons = value;
                OnPropertyChanged("AppbarButtons");
            }
        }

    }
}