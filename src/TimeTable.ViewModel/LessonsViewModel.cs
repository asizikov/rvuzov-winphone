using System;
using System.Collections.ObjectModel;
using JetBrains.Annotations;
using TimeTable.Model;
using TimeTable.ViewModel.Data;
using TimeTable.ViewModel.Services;

namespace TimeTable.ViewModel
{
    public class LessonsViewModel :BaseViewModel
    {
        private readonly INavigationService _navigation;
        private readonly BaseApplicationSettings _applicationSettings;
        private readonly AsyncDataProvider _dataProvider;
        private readonly int _groupId;
        private ObservableCollection<Day> _weekDays;

        public LessonsViewModel([NotNull] INavigationService navigation,
            [NotNull] BaseApplicationSettings applicationSettings, 
            [NotNull] AsyncDataProvider dataProvider, int groupId)
        {
            if (navigation == null) throw new ArgumentNullException("navigation");
            if (applicationSettings == null) throw new ArgumentNullException("applicationSettings");
            if (dataProvider == null) throw new ArgumentNullException("dataProvider");
            _navigation = navigation;
            _applicationSettings = applicationSettings;
            _dataProvider = dataProvider;
            _groupId = groupId;

            Init();
        }

        private void Init()
        {
            IsLoading = true;
            _dataProvider.GetLessonsForGroupAsync(_groupId).Subscribe(
                timeTable =>
                {
                    IsLoading = false;
                    WeekDays = new ObservableCollection<Day>(timeTable.Days);
                },
            ex =>
            {
                IsLoading = false;
            }
            );
        }

        [UsedImplicitly(ImplicitUseKindFlags.Access)]
        public ObservableCollection<Day> WeekDays
        {
            get { return _weekDays; }
            private set
            {
                if (Equals(value, _weekDays)) return;
                _weekDays = value;
                OnPropertyChanged("WeekDays");
            }
        }
    }
}
