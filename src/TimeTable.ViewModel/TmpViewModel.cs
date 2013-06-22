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
        private readonly AsyncDataProvider dataProvider;
        private readonly INavigationService navigation;
        private ObservableCollection<University> universitiesesList;
        private readonly SimpleCommand refreshCommand;
        private University selectedUniversity;

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

        [UsedImplicitly(ImplicitUseKindFlags.Default)]
        public University SelectedUniversity
        {
            get { return selectedUniversity; }
            set
            {
                if (Equals(value, selectedUniversity)) return;
                selectedUniversity = value;
                OnPropertyChanged("SelectedUniversity");
                if (selectedUniversity != null)
                {
                    NavigateToUniversity(selectedUniversity.Id);
                }
            }
        }

        [UsedImplicitly(ImplicitUseKindFlags.Access)]
        public SimpleCommand RefreshCommand { get { return refreshCommand; } }

        private void RefreshList()
        {
            UniversitiesesList = new ObservableCollection<University>();
            Init();
        }


        private void NavigateToUniversity(int id)
        {
            var navigationParameter = new NavigationParameter
            {
                Parameter = NavigationParameterName.Id,
                Value = id.ToString(CultureInfo.InvariantCulture)
            };
            navigation.GoToPage(Pages.Groups, new List<NavigationParameter>{navigationParameter});
        }
    }
}
