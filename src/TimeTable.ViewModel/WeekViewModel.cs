using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using JetBrains.Annotations;
using TimeTable.Model;
using TimeTable.ViewModel.Commands;
using TimeTable.ViewModel.Extensions;
using TimeTable.ViewModel.Utils;

namespace TimeTable.ViewModel
{
    public class WeekViewModel : BaseViewModel
    {
        private readonly WeekType _type;
        private ObservableCollection<DayViewModel> _days;
        private readonly DateTime _parityCountDown;

        public WeekViewModel(IEnumerable<Day> days, long parityCountdown, ICommandFactory commandFactory, WeekType type, University university)
        {
            _parityCountDown = DateTimeUtils.DateTimeFromUnixTimestampSeconds(parityCountdown);
            _type = type;
            WeekNumber = GetWeekNumber();
            Days = new ObservableCollection<DayViewModel>(days.ToViewModelList(commandFactory,_type, university));
        }


        public int WeekNumber
        {
            get; private set;
        }

        private int GetWeekNumber()
        {
            var currentCulture = CultureInfo.InvariantCulture;
            var weekNo = currentCulture.Calendar.GetWeekOfYear(
                DateTime.UtcNow,
                currentCulture.DateTimeFormat.CalendarWeekRule,
                currentCulture.DateTimeFormat.FirstDayOfWeek);
            
            var parityWeekNo = currentCulture.Calendar.GetWeekOfYear(
                _parityCountDown, 
                currentCulture.DateTimeFormat.CalendarWeekRule,
                currentCulture.DateTimeFormat.FirstDayOfWeek);
            weekNo = weekNo - parityWeekNo + 1;
            switch (_type)
            {
                case WeekType.Previous:
                    return (weekNo - 1);
                case WeekType.Current:
                    return weekNo;
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