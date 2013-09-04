using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using JetBrains.Annotations;
using TimeTable.Model;
using TimeTable.ViewModel.Commands;

namespace TimeTable.ViewModel
{
    public class DayViewModel : BaseViewModel
    {
        private readonly Day _day;
        private readonly WeekType _weekType;
        private ObservableCollection<TimeTableItemViewModel> _lessons;

        public DayViewModel([NotNull] Day day, WeekType weekType, [NotNull] ICommandFactory commandFactory)
        {
            if (day == null) throw new ArgumentNullException("day");
            if (commandFactory == null) throw new ArgumentNullException("commandFactory");

            _day = day;
            _weekType = weekType;

            if (_day.Lessons != null)
            {
                Lessons =
                    new ObservableCollection<TimeTableItemViewModel>(
                        _day.Lessons.Select(lesson => new TimeTableItemViewModel(lesson, commandFactory)));
            }
            else
            {
                Lessons = new ObservableCollection<TimeTableItemViewModel>();
            }
            SetUpDayName(_weekType);
        }

        private void SetUpDayName(WeekType weekType)
        {
            var today = DateTime.Now;
            var delta = DayOfWeek.Monday - today.DayOfWeek;
            var monday = today.AddDays(delta);
            

            switch (weekType)
            {
                case WeekType.Previous:
                    monday -= TimeSpan.FromDays(7);
                    break;
                case WeekType.Current:
                    break;
                case WeekType.Next:
                    monday += TimeSpan.FromDays(7);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("weekType");
            }
            NameForTheDay = (monday + TimeSpan.FromDays(_day.Weekday - 1)).ToString("d MMMM");
        }

        [UsedImplicitly(ImplicitUseKindFlags.Access)]
        public string NameForTheDay { get; private set; }

        [NotNull]
        [UsedImplicitly(ImplicitUseKindFlags.Access)]
        public ObservableCollection<TimeTableItemViewModel> Lessons
        {
            get { return _lessons; }
            private set
            {
                if (Equals(value, _lessons)) return;
                _lessons = value;
                OnPropertyChanged("Lessons");
            }
        }

        [UsedImplicitly(ImplicitUseKindFlags.Access)]
        public int Weekday
        {
            get { return _day.Weekday; }
        }
    }
}