using System;
using System.Collections.ObjectModel;
using JetBrains.Annotations;
using TimeTable.Model;
using TimeTable.ViewModel.Commands;
using TimeTable.ViewModel.Data;
using TimeTable.ViewModel.Services;

namespace TimeTable.ViewModel
{
    public class TmpViewModel : BaseViewModel
    {
        private readonly AsyncDataProvider dataProvider;
        private readonly INavigationService navigation;
        private ObservableCollection<University> universitiesesList;
        private readonly SimpleCommand refreshCommand;

        public TmpViewModel([NotNull] AsyncDataProvider dataProvider, [NotNull] INavigationService navigation)
        {
            if (dataProvider == null) throw new ArgumentNullException("dataProvider");
            if (navigation == null) throw new ArgumentNullException("navigation");
            this.dataProvider = dataProvider;
            this.navigation = navigation;
            refreshCommand = new SimpleCommand(RefreshList);
            Init();
        }

        private void Init()
        {
            dataProvider.GetUniversitiesAllAsync().Subscribe(
            result =>
            {
                UniversitiesesList = new ObservableCollection<University>(result.Universities);
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

        [UsedImplicitly(ImplicitUseKindFlags.Access)]
        public ObservableCollection<University> UniversitiesesList
        {
            get { return universitiesesList; }
            private set
            {
                if (Equals(value, universitiesesList)) return;
                universitiesesList = value;
                OnPropertyChanged("UniversitiesesList");
            }
        }

        [UsedImplicitly(ImplicitUseKindFlags.Access)]
        public SimpleCommand RefreshCommand { get { return refreshCommand; } }

        private void RefreshList()
        {
            UniversitiesesList = new ObservableCollection<University>();
            Init();
        }
    }
}
