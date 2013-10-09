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
    public class UniversitiesViewModel : SearchViewModel
    {
        private readonly AsyncDataProvider _dataProvider;
        private readonly FlurryPublisher _flurry;
        private readonly INavigationService _navigation;
        private readonly BaseApplicationSettings _applicationSettings;
        private ObservableCollection<ListGroup<University>> _universitesList;
        private University _selectedUniversity;
        
        private Universities _storedRequest;
        private static Func<University, char> _resultGrouper;

        public UniversitiesViewModel([NotNull] INavigationService navigation,
            [NotNull] BaseApplicationSettings applicationSettings,
            [NotNull] AsyncDataProvider dataProvider,
            [NotNull] FlurryPublisher flurry)
        {
            if (dataProvider == null) throw new ArgumentNullException("dataProvider");
            if (flurry == null) throw new ArgumentNullException("flurry");
            if (navigation == null) throw new ArgumentNullException("navigation");

            _dataProvider = dataProvider;
            _flurry = flurry;
            _navigation = navigation;
            _applicationSettings = applicationSettings;
            _resultGrouper = u => u.ShortName[0];
            SubscribeToQuery();
            Init();
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
                    _flurry.PublishUniversitySelected(_selectedUniversity);
                    NavigateToFaculties(_selectedUniversity.Id);
                }
            }
        }

        private void NavigateToFaculties(int id)
        {
            var navigationParameter = new NavigationParameter
            {
                Parameter = NavigationParameterName.Id,
                Value = id.ToString(CultureInfo.InvariantCulture)
            };
            _navigation.GoToPage(Pages.Faculties, new List<NavigationParameter> {navigationParameter});
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