﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using JetBrains.Annotations;
using TimeTable.Domain;
using TimeTable.Domain.Lessons;
using TimeTable.ViewModel.Factories;

namespace TimeTable.ViewModel
{
    public sealed class WeekViewModel : BaseViewModel
    {
        private ObservableCollection<DayViewModel> _days;
        private DayViewModel _selectedDayItem;

        public WeekViewModel(IEnumerable<Day> days, int weekNumber, [NotNull] DayViewModelFactory dayViewModelFactory,
            WeekType type)
        {
            if (dayViewModelFactory == null) throw new ArgumentNullException("dayViewModelFactory");
            var parity = weekNumber%2;
            WeekNumber = weekNumber;

            Days =
                new ObservableCollection<DayViewModel>(
                    dayViewModelFactory.CreateList(days, type, parity).Where(d => d.Lessons.Any()));
        }

        public int WeekNumber { get; private set; }

        public ObservableCollection<DayViewModel> Days
        {
            get { return _days; }
            private set
            {
                if (Equals(value, _days)) return;
                _days = value;
                OnPropertyChanged("Days");
            }
        }

        public DayViewModel SelectedDayItem
        {
            get { return _selectedDayItem; }
            set
            {
                if (Equals(_selectedDayItem, value)) return;

                _selectedDayItem = value;
                OnPropertyChanged("SelectedDayItem");
            }
        }
    }
}