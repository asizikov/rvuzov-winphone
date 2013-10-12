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
    public class FacultiesPageViewModel : SearchViewModel
    {
        private readonly INavigationService _navigation;
        private readonly BaseApplicationSettings _applicationSettings;
        private readonly AsyncDataProvider _dataProvider;
        private readonly FlurryPublisher _flurryPublisher;
        private readonly int _universityId;
        private ObservableCollection<ListGroup<Faculty>> _facultiesList;
        private Faculty _selectedFaculty;
        private Faculties _storedGroupsRequest;
        private readonly Func<Faculty, char> _facultyGroupFunc;

        public FacultiesPageViewModel([NotNull] INavigationService navigation,
            [NotNull] BaseApplicationSettings applicationSettings, [NotNull] AsyncDataProvider dataProvider,
            [NotNull] FlurryPublisher flurryPublisher, int universityId)
        {
            if (dataProvider == null) throw new ArgumentNullException("dataProvider");
            if (flurryPublisher == null) throw new ArgumentNullException("flurryPublisher");
            if (navigation == null) throw new ArgumentNullException("navigation");
            if (applicationSettings == null) throw new ArgumentNullException("applicationSettings");

            _navigation = navigation;
            _applicationSettings = applicationSettings;
            _dataProvider = dataProvider;
            _flurryPublisher = flurryPublisher;
            _universityId = universityId;

            _applicationSettings.UniversityId = _universityId;

            _facultyGroupFunc = faculty => faculty.Title[0];

            SubscribeToQuery();
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
                            _flurryPublisher.PublishFacultySelected(_selectedFaculty, university);
                            NavigateToGroupsPage(_selectedFaculty);
                        });
                }
            }
        }


        private void Init()
        {
            IsLoading = true;
            _dataProvider.GetUniversitesFacultiesAsync(_universityId).Subscribe(
                result =>
                {
                    _storedGroupsRequest = result;
                    FacultiesList = FormatResult(result.Data, _facultyGroupFunc);
                    IsLoading = false;
                },
                ex => { IsLoading = false; }
                );
        }

        protected override void GetResults(string search)
        {
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
            _applicationSettings.FacultyId = faculty.Id;
            _navigation.GoToPage(Pages.Groups, GetNavitationParameters(faculty));
        }

        private IEnumerable<NavigationParameter> GetNavitationParameters(Faculty faculty)
        {
            return new List<NavigationParameter>
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
        }
    }
}