using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using JetBrains.Annotations;
using TimeTable.Model;

namespace TimeTable.ViewModel
{
    public class DayViewModel : BaseViewModel
    {
        private readonly Day _day;
        private ObservableCollection<TimeTableItemViewModel> _lessons;

        public DayViewModel([NotNull] Day day)
        {
            if (day == null) throw new ArgumentNullException("day");
            _day = day;

            if (_day.Lessons != null)
            {
                Lessons =
                    new ObservableCollection<TimeTableItemViewModel>(
                        _day.Lessons.Select(lesson => new TimeTableItemViewModel(lesson)));
            }
            else
            {
                Lessons = new ObservableCollection<TimeTableItemViewModel>();
            }
        }

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