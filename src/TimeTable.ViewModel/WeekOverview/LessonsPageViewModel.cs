using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using JetBrains.Annotations;
using TimeTable.Domain;
using TimeTable.Domain.Lessons;
using TimeTable.Domain.OrganizationalStructure;
using TimeTable.Domain.Participants;
using TimeTable.Mvvm;
using TimeTable.Mvvm.Navigation;
using TimeTable.ViewModel.ApplicationLevel;
using TimeTable.ViewModel.Commands;
using TimeTable.ViewModel.FavoritedTimeTables;
using TimeTable.ViewModel.MenuItems;
using TimeTable.ViewModel.OrganizationalStructure;
using TimeTable.ViewModel.Services;
using TimeTable.ViewModel.Utils;
using TimeTable.ViewModel.WeekOverview.Factories;

namespace TimeTable.ViewModel.WeekOverview
{
    public sealed class LessonsPageViewModel : PageViewModel<LessonsNavigationParameter>
    {
        private readonly INavigationService _navigation;
        private readonly BaseApplicationSettings _applicationSettings;
        private readonly ICommandFactory _commandFactory;
        private readonly IAsyncDataProvider _dataProvider;
        private readonly FavoritedItemsManager _favoritedItemsManager;
        private readonly IUiStringsProviders _stringsProviders;
        private readonly INotificationService _notificationService;
        private int _id;
        private readonly FlurryPublisher _flurryPublisher;
        private bool _isTeacher;
        private int _selectedWeekIndex;
        private int _facultyId;
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
        private WeekViewModelFactory _weekViewModelFactory;
        private Faculty _faculty;

        public LessonsPageViewModel([NotNull] INavigationService navigation, [NotNull] FlurryPublisher flurryPublisher,
                                    [NotNull] BaseApplicationSettings applicationSettings,
                                    [NotNull] ICommandFactory commandFactory,
                                    [NotNull] IAsyncDataProvider dataProvider,
                                    [NotNull] FavoritedItemsManager favoritedItemsManager,
                                    [NotNull] IUiStringsProviders stringsProviders,
                                    [NotNull] INotificationService notificationService)
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

            Options = new OptionsMonitor();
        }

        public override void Initialize(LessonsNavigationParameter navigationParameter)
        {
            _id = navigationParameter.Id;
            _isTeacher = navigationParameter.IsTeacher;
            _facultyId = navigationParameter.FacultyId;

            InitCommands();
            BuildAppBarButtons();
            UpdateFavoritedSate();
            _flurryPublisher.PublishPageLoadedLessons();
            Init(navigationParameter.UniversityId, navigationParameter.FacultyId);
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
                    IconUri =
                        string.Format("/Resources/Icons/Today/{0}.png", DateTime.Now.Month + "-" + DateTime.Now.Day),
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
                var menuItemsFactory = new LessonMenuItemsFactory(_commandFactory, _university, Options);
                _weekViewModelFactory = new WeekViewModelFactory(menuItemsFactory, _isTeacher, _id);
                if (_isTeacher)
                {
                    _dataProvider.GetTeacherByIdAsync(universityId, _id).Subscribe(teacher =>
                    {
                        _teacher = teacher;
                        Title = (_teacher != null && _teacher.Name != null) ? _teacher.Name.Trim() : string.Empty;
                        UpdateFavoritedSate();
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
                            UpdateFavoritedSate();
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
                _dataProvider.GetLessonsForGroupAsync(_id)
                             .Subscribe(FormatTimeTable, ex => OnError());
            }
        }

        private void OnError()
        {
            IsLoading = false;
            _notificationService.ShowSomethingWentWrongToast();
        }

        private void FormatTimeTable(Domain.Lessons.TimeTable timeTable)
        {
            var weekNumber = 0;
            List<Day> days = null;

            if (timeTable != null && timeTable.Data != null)
            {
                weekNumber = DateTimeUtils.GetRelativeWeekNumber(timeTable.Data.ParityCountdown);
                days = timeTable.Data.Days;
            }

            var navigationFlow = CreateNavigationFlowForFlurry();

            Task.Factory.StartNew(
                () =>
                    CurrentWeek =
                        _weekViewModelFactory.Create(days, weekNumber, WeekType.Current, navigationFlow, _group));
            Task.Factory.StartNew(
                () =>
                    NextWeek = _weekViewModelFactory.Create(days, weekNumber + 1, WeekType.Next, navigationFlow, _group));
            Task.Factory.StartNew(
                () =>
                    PreviousWeek =
                        _weekViewModelFactory.Create(days, weekNumber - 1, WeekType.Previous, navigationFlow, _group));
            IsLoading = false;
        }

        [CanBeNull, UsedImplicitly(ImplicitUseKindFlags.Access)]
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

        [CanBeNull, UsedImplicitly(ImplicitUseKindFlags.Access)]
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

        [CanBeNull, UsedImplicitly(ImplicitUseKindFlags.Access)]
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
            GoToAboutPage = new SimpleCommand(() => _navigation.NavigateTo<AboutViewModel>());
        }

        private void RemoveFromFavorites()
        {
            _favoritedItemsManager.Remove(_isTeacher, _isTeacher ? _teacher.Id : _group.Id, _university, _facultyId);
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
            _navigation.NavigateTo<FavoritesViewModel>();
        }

        private void NavigateToSettingsPage()
        {
            _flurryPublisher.PublishActionbarScheduleSettings(_university, _isTeacher,
                (_isTeacher ? _teacher.Name : _group.GroupName), (_isTeacher ? _teacher.Id : _group.Id));
            _navigation.NavigateTo<SettingsViewModel>();
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
            if (CurrentWeek == null) return;
            var today = DateTime.UtcNow;
            var todayIndex = (int) today.DayOfWeek - 1;

            SelectedWeekIndex = 0;
            CurrentWeek.SelectedDayItem = CurrentWeek.Days.FirstOrDefault(d => d.Weekday == (todayIndex + 1));
            CurrentWeek.SelectedDayItem = null;

            _flurryPublisher.PublishActionbarToday(_university, _isTeacher,
                (_isTeacher ? _teacher.Name : _group.GroupName), (_isTeacher ? _teacher.Id : _group.Id));
        }

        private void UpdateFavoritedSate()
        {
            if (_group == null && _teacher == null)
            {
                FavoritedState = FavoritedState.Unknown;
                return;
            }
// ReSharper disable once PossibleNullReferenceException
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
// ReSharper disable once PossibleNullReferenceException
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

        [UsedImplicitly(ImplicitUseKindFlags.Access)]
        public OptionsMonitor Options { get; private set; }

        private NavigationFlow CreateNavigationFlowForFlurry()
        {
            var navigationFlow = new NavigationFlow
            {
                UniversityId = _university.Id,
                UniversityName = _university.Name,
                FacultyId = _facultyId,
                IsTeacher = _isTeacher
            };
            if (_faculty != null)
            {
                navigationFlow.FacultyName = _faculty.Title;
            }
            return navigationFlow;
        }
    }
}