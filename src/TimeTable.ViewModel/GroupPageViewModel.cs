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
        private readonly INotificationService _notificationService;
        private readonly FavoritedItemsManager _favoritedItemsManager;
        private int _universityId;
        private int _facultyId;
        private Reason _reason;
        private ObservableCollection<ListGroup<Group>> _groupsList;
        private ObservableCollection<ListGroup<Teacher>> _teachersList;
        private Group _selectedGroup;
        private Groups _storedGroupsRequest;
        private Teachers _storedTeachersRequest;
        private int _selectedPivotIndex;
        private readonly Func<Group, char> _groupFunc;
        private readonly Func<Teacher, char> _teachersGroupFunc;
        private Teacher _selectedTeacher;

        public GroupPageViewModel([NotNull] INavigationService navigation,
            [NotNull] BaseApplicationSettings applicationSettings, [NotNull] AsyncDataProvider dataProvider,
            [NotNull] INotificationService notificationService, [NotNull] FlurryPublisher flurryPublisher,
            [NotNull] FavoritedItemsManager favoritedItemsManager):base(flurryPublisher)
        {
            if (dataProvider == null) throw new ArgumentNullException("dataProvider");
            if (notificationService == null) throw new ArgumentNullException("notificationService");
            if (favoritedItemsManager == null) throw new ArgumentNullException("favoritedItemsManager");
            if (navigation == null) throw new ArgumentNullException("navigation");
            if (applicationSettings == null) throw new ArgumentNullException("applicationSettings");

            _navigation = navigation;
            _applicationSettings = applicationSettings;
            _dataProvider = dataProvider;
            _notificationService = notificationService;
            _favoritedItemsManager = favoritedItemsManager;
            
            _groupFunc = group => group.GroupName[0];
            _teachersGroupFunc = teacher => !String.IsNullOrWhiteSpace(teacher.Name) ? teacher.Name[0] : '#';

            SubscribeToQuery();

        }

        public void Initialize(int universityId, int facultyId, Reason reason)
        {
            _universityId = universityId;
            _facultyId = facultyId;
            _reason = reason;
            FlurryPublisher.PublishPageLoadedGroups();
            Init();
        }

        [UsedImplicitly(ImplicitUseKindFlags.Access)]
        public ObservableCollection<ListGroup<Group>> GroupsList
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
        public ObservableCollection<ListGroup<Teacher>> TeachersList
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
                            FlurryPublisher.PublishGroupSelected(_selectedGroup, university);
                            NavigateToLessonsPage(_selectedGroup, university);
                        });
                }
            }
        }

        [UsedImplicitly(ImplicitUseKindFlags.Default)]
        public Teacher SelectedTeacher
        {
            get { return _selectedTeacher; }
            set
            {
                if (Equals(value, _selectedTeacher)) return;
                _selectedTeacher = value;
                OnPropertyChanged("SelectedTeacher");
                if (_selectedTeacher != null)
                {
                    _dataProvider.GetUniversityByIdAsync(_universityId)
                        .Subscribe(university =>
                        {
                            FlurryPublisher.PublishTeacherSelected(_selectedTeacher, university);
                            NavigateToLessonsPage(_selectedTeacher, university);
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
            _dataProvider.GetFacultyGroupsAsync(_facultyId).Subscribe(
                result =>
                {
                    result.GroupsList = result.GroupsList.OrderBy(g => g.GroupName).ToList();
                    _dataProvider.PutGroups(_universityId, _facultyId, result.GroupsList);
                    _storedGroupsRequest = result;
                    GroupsList = FormatResult(result.GroupsList, _groupFunc);
                    IsLoading = false;
                },
                ex => OnError());

            _dataProvider.GetUniversityTeachersAsync(_universityId).Subscribe(
                result =>
                {
                    _storedTeachersRequest = result;
                    TeachersList = FormatResult(result.TeachersList, _teachersGroupFunc);
                }, ex => OnError()
                );
        }

        private void OnError()
        {
            IsLoading = false;
            _notificationService.ShowSomethingWentWrongToast();
        }

        protected override void GetResults(string search)
        {
            if (String.IsNullOrEmpty(search))
            {
                GroupsList = FormatResult(_storedGroupsRequest.GroupsList, _groupFunc);
                TeachersList = FormatResult(_storedTeachersRequest.TeachersList, _teachersGroupFunc);
                return;
            }
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
                        _storedTeachersRequest.TeachersList.Where(teacher => teacher.Name != null && teacher.Name.IgnoreCaseContains(search)),
                        _teachersGroupFunc);
            }
        }

        private void NavigateToLessonsPage(Group group, University university)
        {
            switch (_reason)
            {
                case Reason.Registration:
                    if (!_applicationSettings.IsRegistrationCompleted)
                    {
                        _applicationSettings.Me.DefaultGroup = group;
                        _navigation.GoToPage(Pages.Lessons, GetNavitationParameters(group), 4);
                    }
                    break;
                case Reason.AddingFavorites:
                    AddGoupToFavorites(group, university);
                    break;
                case Reason.ChangeDefault:
                    _applicationSettings.Me.University = _applicationSettings.Me.TemporaryUniversity;
                    _applicationSettings.Me.Faculty = _applicationSettings.Me.TemporaryFaculty;
                    _applicationSettings.Me.DefaultGroup = group;
                    _applicationSettings.Me.Teacher = null;
                    _applicationSettings.Me.TemporaryFaculty = null;
                    _applicationSettings.Me.TemporaryUniversity = null;
                    _navigation.GoToPage(Pages.Lessons, GetNavitationParameters(group), 5);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void NavigateToLessonsPage(Teacher teacher, University university)
        {
            switch (_reason)
            {
                case Reason.Registration:
                    if (!_applicationSettings.IsRegistrationCompleted)
                    {
                        _applicationSettings.Me.Teacher = teacher;
                        _navigation.GoToPage(Pages.Lessons, GetNavitationParameters(teacher), 4);
                    }
                    break;
                case Reason.AddingFavorites:
                    AddTeacherToFavorites(teacher, university);
                    break;
                case Reason.ChangeDefault:
                    _applicationSettings.Me.University = _applicationSettings.Me.TemporaryUniversity;
                    _applicationSettings.Me.Faculty = _applicationSettings.Me.TemporaryFaculty;
                    _applicationSettings.Me.Teacher = teacher;
                    _applicationSettings.Me.DefaultGroup = null;
                    _applicationSettings.Me.TemporaryFaculty = null;
                    _applicationSettings.Me.TemporaryUniversity = null;
                    _navigation.GoToPage(Pages.Lessons, GetNavitationParameters(teacher), 5);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void AddGoupToFavorites(Group group, University university)
        {
            _favoritedItemsManager.Add(false, group.Id, group.GroupName, university, _facultyId);
            _navigation.GoToPage(Pages.FarovitesPage, null, 4);
        }

        private void AddTeacherToFavorites(Teacher teacher, University university)
        {
            _favoritedItemsManager.Add(true, teacher.Id, teacher.Name, university, _facultyId);
            _navigation.GoToPage(Pages.FarovitesPage, null, 4);
        }

        private IEnumerable<NavigationParameter> GetNavitationParameters(Group group)
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
                },
                new NavigationParameter
                {
                    Parameter = NavigationParameterName.UniversityId,
                    Value = _universityId.ToString(CultureInfo.InvariantCulture)
                },
                new NavigationParameter
                {
                    Parameter = NavigationParameterName.FacultyId,
                    Value = _facultyId.ToString(CultureInfo.InvariantCulture)
                }
            };
        }

        private IEnumerable<NavigationParameter> GetNavitationParameters(Teacher teacher)
        {
            return new List<NavigationParameter>
            {
                new NavigationParameter
                {
                    Parameter = NavigationParameterName.Id,
                    Value = teacher.Id.ToString(CultureInfo.InvariantCulture)
                },
                new NavigationParameter
                {
                    Parameter = NavigationParameterName.IsTeacher,
                    Value = true.ToString()
                },
                new NavigationParameter
                {
                    Parameter = NavigationParameterName.UniversityId,
                    Value = _universityId.ToString(CultureInfo.InvariantCulture)
                },
                new NavigationParameter
                {
                    Parameter = NavigationParameterName.FacultyId,
                    Value = _facultyId.ToString(CultureInfo.InvariantCulture)
                }
            };
        }
    }
}