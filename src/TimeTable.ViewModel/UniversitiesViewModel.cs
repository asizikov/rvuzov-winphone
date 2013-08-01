using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using JetBrains.Annotations;
using TimeTable.Model;
using TimeTable.ViewModel.Data;
using TimeTable.ViewModel.Services;

namespace TimeTable.ViewModel
{
    public class UniversitiesViewModel : BaseViewModel
    {
        private readonly AsyncDataProvider _dataProvider;
        private readonly FlurryPublisher _flurry;
        private readonly INavigationService _navigation;
        private readonly BaseApplicationSettings _applicationSettings;
        private ObservableCollection<ListGroup<University>> _universitesList;
        private University _selectedUniversity;

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

            Init();
        }


        private void Init()
        {
            IsLoading = true;
            _dataProvider.GetUniversitesAsync().Subscribe(
                result =>
                    {
                        UniversitesList = FormatResult(result);
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

        private static ObservableCollection<ListGroup<University>> FormatResult(Universities result)
        {
            var grouped = result.UniversitesList
                .GroupBy(u => u.ShortName[0])
                .Select(g => new ListGroup<University>(g.Key.ToString(CultureInfo.InvariantCulture),
                    g.ToList()));
            var observableCollection = new ObservableCollection<ListGroup<University>>(grouped);
            return observableCollection;
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
                _flurry.PublishUniversitySelected(_selectedUniversity);
                if (_selectedUniversity != null)
                {
                    NavigateToUniversity(_selectedUniversity.Id);
                }
            }
        }


        private void NavigateToUniversity(int id)
        {
            var navigationParameter = new NavigationParameter
                {
                    Parameter = NavigationParameterName.Id,
                    Value = id.ToString(CultureInfo.InvariantCulture)
                };
            _navigation.GoToPage(Pages.Groups, new List<NavigationParameter> {navigationParameter});
        }
    }
}