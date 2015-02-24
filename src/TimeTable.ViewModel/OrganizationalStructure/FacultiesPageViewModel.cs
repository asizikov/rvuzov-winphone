using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using JetBrains.Annotations;
using TimeTable.Domain;
using TimeTable.Domain.OrganizationalStructure;
using TimeTable.ViewModel.Services;
using TimeTable.ViewModel.Utils;

namespace TimeTable.ViewModel.OrganizationalStructure
{
    public class FacultiesPageViewModel : SearchViewModel
    {
        private readonly INavigationService _navigation;
        private readonly BaseApplicationSettings _applicationSettings;
        private readonly IAsyncDataProvider _dataProvider;
        private readonly INotificationService _notificationService;
        private int _universityId;
        private Reason _reason;
        private ObservableCollection<ListGroup<Faculty>> _facultiesList;
        private Faculty _selectedFaculty;
        private Faculties _storedGroupsRequest;
        private readonly Func<Faculty, char> _facultyGroupFunc;

        public FacultiesPageViewModel([NotNull] INavigationService navigation,
            [NotNull] BaseApplicationSettings applicationSettings, [NotNull] IAsyncDataProvider dataProvider,
            [NotNull] FlurryPublisher flurryPublisher, [NotNull] INotificationService notificationService) :base(flurryPublisher)
        {
            if (dataProvider == null) throw new ArgumentNullException("dataProvider");
            if (flurryPublisher == null) throw new ArgumentNullException("flurryPublisher");
            if (notificationService == null) throw new ArgumentNullException("notificationService");
            if (navigation == null) throw new ArgumentNullException("navigation");
            if (applicationSettings == null) throw new ArgumentNullException("applicationSettings");

            _navigation = navigation;
            _applicationSettings = applicationSettings;
            _dataProvider = dataProvider;
            _notificationService = notificationService;
            _facultyGroupFunc = faculty => faculty.Title[0];
            SubscribeToQuery();
        }

        public void Initialize(int universityId, Reason reason)
        {
            _universityId = universityId;
            _reason = reason;
            FlurryPublisher.PublishPageLoadedFaculties();
            Init();
        }

        [UsedImplicitly(ImplicitUseKindFlags.Access)]
        public ObservableCollection<ListGroup<Faculty>> FacultiesList
        {
            get { return _facultiesList; }
            private set
            {
                if (Equals(value, _facultiesList)) return;
                _facultiesList = value;
                OnPropertyChanged("FacultiesList");
            }
        }

        [UsedImplicitly(ImplicitUseKindFlags.Default)]
        public Faculty SelectedFaculty
        {
            get { return _selectedFaculty; }
            set
            {
                if (Equals(value, _selectedFaculty)) return;
                _selectedFaculty = value;
                OnPropertyChanged("SelectedFaculty");
                if (_selectedFaculty != null)
                {
                    _dataProvider.GetUniversityByIdAsync(_universityId)
                        .Subscribe(university =>
                        {
                            FlurryPublisher.PublishFacultySelected(_selectedFaculty, university);
                            NavigateToGroupsPage(_selectedFaculty);
                        });
                }
            }
        }


        private void Init()
        {
            IsLoading = true;
            _dataProvider.GetUniversityFacultiesAsync(_universityId).Subscribe(
                result =>
                {
                    if (!result.Success) return;

                    _storedGroupsRequest = result;
                    _dataProvider.PutFaculties(_universityId, result.Data);
                    FacultiesList = FormatResult(result.Data, _facultyGroupFunc);
                    IsLoading = false;
                },
                ex =>
                {
                    IsLoading = false;
                    _notificationService.ShowSomethingWentWrongToast();
                }
                );
        }

        protected override void GetResults(string search)
        {
            if (_storedGroupsRequest == null || _storedGroupsRequest.Data == null)
            {
                return;
            }
            if (String.IsNullOrEmpty(search))
            {
                FacultiesList = FormatResult(_storedGroupsRequest.Data, _facultyGroupFunc);
                return;
            }
            FacultiesList =
                FormatResult(_storedGroupsRequest.Data.Where(u => u.Title.IgnoreCaseContains(search)),
                    _facultyGroupFunc);
        }

        private void NavigateToGroupsPage(Faculty faculty)
        {
            if (!_applicationSettings.IsRegistrationCompleted)
            {
                _applicationSettings.Me.Faculty = faculty;
                _applicationSettings.Save();
            }
            _navigation.GoToPage(Pages.Groups, GetNavigationParameters(faculty));
        }

        private IEnumerable<NavigationParameter> GetNavigationParameters(Faculty faculty)
        {
            var list = new List<NavigationParameter>
            {
                new NavigationParameter
                {
                    Parameter = NavigationParameterName.UniversityId,
                    Value = _universityId.ToString(CultureInfo.InvariantCulture)
                },
                new NavigationParameter
                {
                    Parameter = NavigationParameterName.Id,
                    Value = faculty.Id.ToString(CultureInfo.InvariantCulture)
                },
                new NavigationParameter
                {
                    Parameter = NavigationParameterName.IsTeacher,
                    Value = false.ToString()
                },
                new NavigationParameter
                {
                    Parameter = NavigationParameterName.Name,
                    Value = faculty.Title
                },
            };
            if (_reason == Reason.AddingFavorites)
            {
                list.Add(new NavigationParameter
                {
                    Parameter = NavigationParameterName.AddFavorites,
                    Value = true.ToString()
                });
            }
            else if (_reason == Reason.ChangeDefault)
            {
                _applicationSettings.Me.TemporaryFaculty = faculty;
                list.Add(new NavigationParameter
                {
                    Parameter = NavigationParameterName.ChangeDefault,
                    Value = true.ToString()
                });
            }
            return list;
        }
    }
}