using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using JetBrains.Annotations;
using TimeTable.Model;
using TimeTable.ViewModel.Extensions;

namespace TimeTable.ViewModel
{
    public class WeekViewModel : BaseViewModel
    {
        private readonly WeekType _type;
        private ObservableCollection<DayViewModel> _days;

        public WeekViewModel(IEnumerable<Day> days, WeekType type)
        {
            _type = type;
            Days = new ObservableCollection<DayViewModel>(days.ToViewModelList(_type));
        }


        public int WeekNumber
        {
            get { return GetWeekNumber(); }
        }

        private int GetWeekNumber()
        {
            var currentCulture = CultureInfo.CurrentCulture;
            var weekNo = currentCulture.Calendar.GetWeekOfYear(
                DateTime.Now,
                currentCulture.DateTimeFormat.CalendarWeekRule,
                currentCulture.DateTimeFormat.FirstDayOfWeek);
            switch (_type)
            {
                case WeekType.Previous:
                    return (weekNo - 1);
                case WeekType.Current:
                    return weekNo;

                    break;
                case WeekType.Next:
                    return (weekNo + 1);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        [UsedImplicitly(ImplicitUseKindFlags.Access)]
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
    }
}