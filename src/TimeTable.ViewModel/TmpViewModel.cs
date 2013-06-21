using System;
using System.Collections.ObjectModel;
using JetBrains.Annotations;
using TimeTable.Model;
using TimeTable.ViewModel.Commands;
using TimeTable.ViewModel.Data;

namespace TimeTable.ViewModel
{
    public class TmpViewModel : BaseViewModel
    {
        private readonly AsyncDataProvider dataProvider;
        private ObservableCollection<University> universitiesesList;
        private readonly SimpleCommand refreshCommand;

        public TmpViewModel([NotNull] AsyncDataProvider dataProvider)
        {
            if (dataProvider == null) throw new ArgumentNullException("dataProvider");
            this.dataProvider = dataProvider;
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
