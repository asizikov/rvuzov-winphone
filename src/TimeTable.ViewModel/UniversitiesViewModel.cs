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
    public enum Reason
    {
        Registration = 0,
        AddingFavorites = 1,
        ChangeDefault = 2
    }

    public class UniversitiesViewModel : SearchViewModel
    {
        private readonly AsyncDataProvider _dataProvider;
        private readonly FlurryPublisher _flurry;
        private readonly Reason _reason;
        private readonly INavigationService _navigation;
        private readonly BaseApplicationSettings _applicationSettings;
        private ObservableCollection<ListGroup<University>> _universitesList;
        private University _selectedUniversity;

        private Universities _storedRequest;
        private static Func<University, char> _resultGrouper;

        public UniversitiesViewModel([NotNull] INavigationService navigation,
            [NotNull] BaseApplicationSettings applicationSettings, [NotNull] AsyncDataProvider dataProvider,
            [NotNull] FlurryPublisher flurry, Reason reason)
        {
            if (dataProvider == null) throw new ArgumentNullException("dataProvider");
            if (flurry == null) throw new ArgumentNullException("flurry");
            if (navigation == null) throw new ArgumentNullException("navigation");

            _dataProvider = dataProvider;
            _flurry = flurry;
            _reason = reason;
            _navigation = navigation;
            _applicationSettings = applicationSettings;
            _resultGrouper = u => u.ShortName[0];
            SubscribeToQuery();
            Init();
            _flurry.PublishPageLoadedUniversities();
        }


        private void Init()
        {
            IsLoading = true;
            _dataProvider.GetUniversitesAsync().Subscribe(
                result =>
                {
                    result.Data = result.Data.OrderBy(u => u.ShortName).ToList();
                    _storedRequest = result;
                    UniversitesList = FormatResult(result.Data, _resultGrouper);
                    IsLoading = false;
                },
                ex =>
                {
                    IsLoading = false;
                    //handle exception
                },
                () =>
                {
                    //handle loaded
                }
                );
        }

        [UsedImplicitly(ImplicitUseKindFlags.Access)]
        public ObservableCollection<ListGroup<University>> UniversitesList
        {
            get { return _universitesList; }
            private set
            {
                if (Equals(value, _universitesList)) return;
                _universitesList = value;
                OnPropertyChanged("UniversitesList");
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
                    _flurry.PublishUniversitySelected(_selectedUniversity);
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
            _navigation.GoToPage(Pages.Faculties, parameters);
        }

        protected override void GetResults(string search)
        {
            UniversitesList = FormatResult(
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