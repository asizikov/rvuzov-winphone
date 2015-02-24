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
    public enum Reason
    {
        Registration = 0,
        AddingFavorites = 1,
        ChangeDefault = 2
    }

    public class UniversitiesViewModel : SearchViewModel
    {
        private readonly IAsyncDataProvider _dataProvider;
        private readonly INotificationService _notificationService;
        private readonly Mvvm.Navigation.INavigationService _navigation;
        private readonly BaseApplicationSettings _applicationSettings;
        private ObservableCollection<ListGroup<University>> _universitiesList;
        private University _selectedUniversity;

        private Universities _storedRequest;
        private static Func<University, char> _resultGrouper;
        private Reason _reason;

        public UniversitiesViewModel([NotNull] Mvvm.Navigation.INavigationService navigation,
            [NotNull] BaseApplicationSettings applicationSettings, [NotNull] IAsyncDataProvider dataProvider,
            [NotNull] FlurryPublisher flurry, [NotNull] INotificationService notificationService) :base(flurry)
        {
            if (dataProvider == null) throw new ArgumentNullException("dataProvider");
            if (notificationService == null) throw new ArgumentNullException("notificationService");
            if (navigation == null) throw new ArgumentNullException("navigation");

            _dataProvider = dataProvider;
            _notificationService = notificationService;
            _navigation = navigation;
            _applicationSettings = applicationSettings;
            _resultGrouper = u => u.ShortName[0];
            SubscribeToQuery();
        }

        public void Initialize(Reason reason)
        {
            _reason = reason;
            Init();
            FlurryPublisher.PublishPageLoadedUniversities();
        }


        private void Init()
        {
            IsLoading = true;
            _dataProvider.GetUniversitiesAsync().Subscribe(
                result =>
                {
                    var filtered = result.Data.Where(u => !string.IsNullOrWhiteSpace(u.ShortName)).ToList();
                    filtered.ForEach(u => u.ShortName = u.ShortName.Trim());
                    result.Data = filtered.OrderBy(u => u.ShortName).ToList();
                    _storedRequest = result;
                    UniversitiesList = FormatResult(result.Data, _resultGrouper);
                    IsLoading = false;
                },
                ex =>
                {
                    IsLoading = false;
                    _notificationService.ShowSomethingWentWrongToast();
                }
                );
        }

        [UsedImplicitly(ImplicitUseKindFlags.Access)]
        public ObservableCollection<ListGroup<University>> UniversitiesList
        {
            get { return _universitiesList; }
            private set
            {
                if (Equals(value, _universitiesList)) return;
                _universitiesList = value;
                OnPropertyChanged("UniversitiesList");
            }
        }

        [UsedImplicitly(ImplicitUseKindFlags.Default)]
        public University SelectedUniversity
        {
            get { return _selectedUniversity; }
            set
            {
                if (Equals(value, _selectedUniversity)) return;
                _selectedUniversity = value;
                OnPropertyChanged("SelectedUniversity");

                if (_selectedUniversity != null)
                {
                    _dataProvider.PutUniversity(_selectedUniversity);
                    FlurryPublisher.PublishUniversitySelected(_selectedUniversity);
                    NavigateToFaculties(_selectedUniversity);
                }
            }
        }

        private void NavigateToFaculties(University university)
        {
            var navigationParameter = new NavigationParameter
            {
                Parameter = NavigationParameterName.Id,
                Value = university.Id.ToString(CultureInfo.InvariantCulture)
            };
            if (!_applicationSettings.IsRegistrationCompleted)
            {
                _applicationSettings.Me.University = university;
                _applicationSettings.Save();
            }
            var parameters = new List<NavigationParameter> {navigationParameter};
            if (_reason == Reason.AddingFavorites)
            {
                parameters.Add(new NavigationParameter
                {
                    Parameter = NavigationParameterName.AddFavorites,
                    Value = true.ToString()
                });
            }
            else if (_reason == Reason.ChangeDefault)
            {
                _applicationSettings.Me.TemporaryUniversity = university;
                parameters.Add(new NavigationParameter
                {
                    Parameter = NavigationParameterName.ChangeDefault,
                    Value = true.ToString()
                });
            }
            var facultyParameter = new FacultyParameter();
            facultyParameter.UniversityId = university.Id;
            facultyParameter.Reason = _reason;
            _navigation.NavigateTo<FacultiesPageViewModel,FacultyParameter>(facultyParameter);
        }

        protected override void GetResults(string search)
        {
            if (_storedRequest == null || _storedRequest.Data == null)
            {
                return;
            }

            UniversitiesList = FormatResult(
                String.IsNullOrEmpty(search)
                    ? _storedRequest.Data
                    : _storedRequest.Data.Where(u => Matches(u, search)), _resultGrouper);
        }

        private static bool Matches(University university, string search)
        {
            return university.Name.IgnoreCaseContains(search) ||
                   university.ShortName.IgnoreCaseContains(search);
        }
    }
}