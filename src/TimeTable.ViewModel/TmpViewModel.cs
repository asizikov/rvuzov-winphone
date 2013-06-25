using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using JetBrains.Annotations;
using TimeTable.Model;
using TimeTable.ViewModel.Commands;
using TimeTable.ViewModel.Data;
using TimeTable.ViewModel.Services;

namespace TimeTable.ViewModel
{
    public class TmpViewModel : BaseViewModel
    {
        private readonly AsyncDataProvider _dataProvider;
        private readonly INavigationService _navigation;
        private ObservableCollection<University> _universitesList;
        private readonly SimpleCommand _refreshCommand;
        private University _selectedUniversity;


        public TmpViewModel([NotNull] INavigationService navigation,
                            [NotNull] BaseApplicationSettings applicationSettings,
                            [NotNull] AsyncDataProvider dataProvider)
        {
            if (dataProvider == null) throw new ArgumentNullException("dataProvider");
            if (navigation == null) throw new ArgumentNullException("navigation");

            _dataProvider = dataProvider;
            _navigation = navigation;

            _refreshCommand = new SimpleCommand(RefreshList);

            Init();
        }

        private void Init()
        {
            _dataProvider.GetUniversitesAsync().Subscribe(
                result => { UniversitesList = new ObservableCollection<University>(result.UniversitesList); },
                ex =>
                    {
                        //handle exception
                    },
                () =>
                    {
                        //handle loaded
                    }
                );
        }

        [UsedImplicitly(ImplicitUseKindFlags.Access)]
        public ObservableCollection<University> UniversitesList
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
                    NavigateToUniversity(_selectedUniversity.Id);
                }
            }
        }

        [UsedImplicitly(ImplicitUseKindFlags.Access)]
        public SimpleCommand RefreshCommand
        {
            get { return _refreshCommand; }
        }

        private void RefreshList()
        {
            UniversitesList = new ObservableCollection<University>();
            Init();
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