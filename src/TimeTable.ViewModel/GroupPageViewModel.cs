using System;
using System.Collections.ObjectModel;
using JetBrains.Annotations;
using TimeTable.Model;
using TimeTable.ViewModel.Data;
using TimeTable.ViewModel.Services;

namespace TimeTable.ViewModel
{
    public class GroupPageViewModel : BaseViewModel
    {
        private readonly INavigationService _navigation;
        private readonly BaseApplicationSettings _applicationSettings;
        private readonly AsyncDataProvider _dataProvider;
        private readonly int _universityId;
        private ReadOnlyObservableCollection<Group> _groupsList;

        public GroupPageViewModel([NotNull] INavigationService navigation,
                                  [NotNull] BaseApplicationSettings applicationSettings,
                                  [NotNull] AsyncDataProvider dataProvider,
                                  int universityId)
        {
            if (dataProvider == null) throw new ArgumentNullException("dataProvider");
            if (navigation == null) throw new ArgumentNullException("navigation");

            _navigation = navigation;
            _applicationSettings = applicationSettings;
            _dataProvider = dataProvider;
            _universityId = universityId;

            Init();
        }

        [UsedImplicitly(ImplicitUseKindFlags.Access)]
        public ReadOnlyObservableCollection<Group> GroupsList
        {
            get { return _groupsList; }
            set
            {
                if (Equals(value, _groupsList)) return;
                _groupsList = value;
                OnPropertyChanged("GroupsList");
            }
        }

        private void Init()
        {
            IsLoading = true;
            _dataProvider.GetUniversitesGroupsAsync(_universityId).Subscribe(
                result =>
                {
                    IsLoading = true;
                    GroupsList = new ReadOnlyObservableCollection<Group>(
                        new ObservableCollection<Group>(result.GroupsList));
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
    }
}