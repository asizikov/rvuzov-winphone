using System;
using System.Collections.ObjectModel;
using System.Linq;
using JetBrains.Annotations;
using TimeTable.Model;
using TimeTable.ViewModel.Data;
using TimeTable.ViewModel.Extensions;
using TimeTable.ViewModel.Services;

namespace TimeTable.ViewModel
{
    public class LessonsViewModel : BaseViewModel
    {
        private readonly INavigationService _navigation;
        private readonly BaseApplicationSettings _applicationSettings;
        private readonly AsyncDataProvider _dataProvider;
        private readonly Group _group;
        private ObservableCollection<DayViewModel> _weekDays;

        public LessonsViewModel([NotNull] INavigationService navigation,
            [NotNull] BaseApplicationSettings applicationSettings,
            [NotNull] AsyncDataProvider dataProvider, int groupId, string groupName)
        {
            if (navigation == null) throw new ArgumentNullException("navigation");
            if (applicationSettings == null) throw new ArgumentNullException("applicationSettings");
            if (dataProvider == null) throw new ArgumentNullException("dataProvider");
            _navigation = navigation;
            _applicationSettings = applicationSettings;
            _dataProvider = dataProvider;
            _group = new Group { GroupName = groupName, Id = groupId };

            _applicationSettings.GroupId = _group.Id;

            Init();
        }

        private void Init()
        {
            IsLoading = true;
            _dataProvider.GetLessonsForGroupAsync(_group.Id).Subscribe(
                timeTable =>
                {
                    IsLoading = false;
                    WeekDays = new ObservableCollection<DayViewModel>(timeTable.Days.ToViewModelList());
                },
            ex =>
            {
                IsLoading = false;
            }
            );
        }

        [UsedImplicitly(ImplicitUseKindFlags.Access)]
        public ObservableCollection<DayViewModel> WeekDays
        {
            get { return _weekDays; }
            private set
            {
                if (Equals(value, _weekDays)) return;
                _weekDays = value;
                OnPropertyChanged("WeekDays");
            }
        }


        [UsedImplicitly(ImplicitUseKindFlags.Access)]
        public String GroupName
        {
            get { return _group.GroupName; }
        }
    }
}
