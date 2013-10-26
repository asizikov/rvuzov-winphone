using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Threading;
using JetBrains.Annotations;
using TimeTable.Model;
using TimeTable.ViewModel.Commands;
using TimeTable.ViewModel.Data;
using TimeTable.ViewModel.Factories;
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
        private readonly INotificationService _notificationService;
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
        private Faculty _faculty;
        private WeekViewModelFactory _weekViewModelFactory;

        public LessonsViewModel([NotNull] INavigationService navigation, [NotNull] FlurryPublisher flurryPublisher, [NotNull] BaseApplicationSettings applicationSettings, [NotNull] ICommandFactory commandFactory, [NotNull] AsyncDataProvider dataProvider, [NotNull] FavoritedItemsManager favoritedItemsManager, [NotNull] IUiStringsProviders stringsProviders,
            [NotNull] INotificationService notificationService, int id, bool isTeacher, int universityId, int facultyId)

        {
            if (navigation == null) throw new ArgumentNullException("navigation");
            if (applicationSettings == null) throw new ArgumentNullException("applicationSettings");
            if (commandFactory == null) throw new ArgumentNullException("commandFactory");
            if (dataProvider == null) throw new ArgumentNullException("dataProvider");
            if (favoritedItemsManager == null) throw new ArgumentNullException("favoritedItemsManager");
            if (stringsProviders == null) throw new ArgumentNullException("stringsProviders");
            if (notificationService == null) throw new ArgumentNullException("notificationService");

            _navigation = navigation;
            _applicationSettings = applicationSettings;
            _flurryPublisher = flurryPublisher;
            _commandFactory = commandFactory;
            _dataProvider = dataProvider;
            _favoritedItemsManager = favoritedItemsManager;
            _stringsProviders = stringsProviders;
            _notificationService = notificationService;
            _id = id;
            _isTeacher = isTeacher;
            _facultyId = facultyId;
            _flurryPublisher.PublishPageLoadedLessons();
            Init(universityId, facultyId);

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

        private void Init(int universityId, int facultyId)
        {
            IsLoading = true;
            _dataProvider.GetUniversityByIdAsync(universityId).Subscribe(university =>
            {
                _university = university;
                _weekViewModelFactory = new WeekViewModelFactory(_commandFactory, _university, _isTeacher, _id);
                if (_isTeacher)
                {
                    _dataProvider.GetTeacherByIdAsync(universityId, _id).Subscribe(teacher =>
                    {
                        _teacher = teacher;
                        Title = (_teacher != null && _teacher.Name != null) ? _teacher.Name.Trim() : string.Empty;
                        UpdateFaforitedSate();
                    });
                    LoadLessons();
                }
                else
                {
                    _dataProvider.GetFacultyByIdAsync(universityId, facultyId).Subscribe(faculty =>
                    {
                        _faculty = faculty;
                        //we need to go deeper :)
                        _dataProvider.GetGroupByIdAsync(_facultyId, _id).Subscribe(group =>
                        {
                            _group = group;
                            Title = (_group != null && _group.GroupName != null)
                                ? _group.GroupName.Trim()
                                : string.Empty;
                            UpdateFaforitedSate();
                        });
                        LoadLessons();
                    });
                }
            });
        }

        private void LoadLessons()
        {
            if (_isTeacher)
            {
                _dataProvider.GetLessonsForTeacherAsync(_id)
                    .Subscribe(FormatTimeTable, ex => OnError());
            }
            else
            {
                _dataProvider.GetLessonsForGroupAsync(_id).Subscribe(
                    FormatTimeTable,
                    ex => OnError()
                    );
            }
        }

        private void OnError()
        {
            IsLoading = false;
            _notificationService.ShowSomethingWentWrongToast();
        }

        private void FormatTimeTable(Model.TimeTable timeTable)
        {
            var weekNumber = DateTimeUtils.GetRelativeWeekNumber(timeTable.Data.ParityCountdown);

            CurrentWeek = _weekViewModelFactory.Create(timeTable.Data.Days, weekNumber, WeekType.Current);
            NextWeek = _weekViewModelFactory.Create(timeTable.Data.Days, weekNumber + 1, WeekType.Next);
            PreviousWeek = _weekViewModelFactory.Create(timeTable.Data.Days, weekNumber - 1, WeekType.Previous);
            IsLoading = false;
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

        [UsedImplicitly(ImplicitUseKindFlags.Access)]
        public ICommand GoToTodayCommand { get; private set; }

        [UsedImplicitly(ImplicitUseKindFlags.Access)]
        public ICommand AddToFavoritesCommand { get; private set; }

        [UsedImplicitly(ImplicitUseKindFlags.Access)]
        public ICommand RemoveFromFavoritesCommand { get; private set; }


        [UsedImplicitly(ImplicitUseKindFlags.Access)]
        public ICommand GoToAboutPage { get; private set; }

        private void InitCommands()
        {
            GoToSettingsCommand = new SimpleCommand(NavigateToSettingsPage);
            GoToFavoritesListCommand = new SimpleCommand(NavigateToFavoritesPage);

            GoToTodayCommand = new SimpleCommand(SelectTodayItem);
            AddToFavoritesCommand = new SimpleCommand(AddToFavorites);
            RemoveFromFavoritesCommand = new SimpleCommand(RemoveFromFavorites);
            GoToAboutPage = new SimpleCommand(() => { _navigation.GoToPage(Pages.AboutPage); });
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
            {
                if (Equals(value, _selectedWeekIndex)) return;
                _selectedWeekIndex = value;
                OnPropertyChanged("SelectedWeekIndex");
            }
        }

        private void SelectTodayItem()
        {
            DateTime today = DateTime.UtcNow;
            int todayIndex = (int) today.DayOfWeek - 1;

            SelectedWeekIndex = 0;
            CurrentWeek.SelectedDayItem = CurrentWeek.Days.FirstOrDefault(d => d.Weekday == (todayIndex + 1));
            CurrentWeek.SelectedDayItem = null;

            _flurryPublisher.PublishActionbarToday(_university, _isTeacher,
                (_isTeacher ? _teacher.Name : _group.GroupName), (_isTeacher ? _teacher.Id : _group.Id));
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