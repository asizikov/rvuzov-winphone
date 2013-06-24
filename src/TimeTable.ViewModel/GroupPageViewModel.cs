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
        private readonly AsyncDataProvider _dataProvider;
        private readonly INavigationService _navigation;
        private readonly int _parameter;
        private ObservableCollection<Group> _groupsList;

        public GroupPageViewModel([NotNull] AsyncDataProvider dataProvider, [NotNull] INavigationService navigation, int parameter)
        {
            if (dataProvider == null) throw new ArgumentNullException("dataProvider");
            if (navigation == null) throw new ArgumentNullException("navigation");

            _dataProvider = dataProvider;
            _navigation = navigation;
            _parameter = parameter;

            Init();
        }

        [UsedImplicitly(ImplicitUseKindFlags.Access)]
        public ObservableCollection<Group> GroupsList
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
            _dataProvider.GetUniversitesGroupsAsync(_parameter).Subscribe(
                result =>
                    {
                        GroupsList = new ObservableCollection<Group>(result.GroupsList);
//                        GroupsList = new ReadOnlyObservableCollection<Group>(
//                            new ObservableCollection<Group>(result.GroupsList));
                    },
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
    }
}