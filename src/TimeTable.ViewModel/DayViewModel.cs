using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using JetBrains.Annotations;
using TimeTable.Model;
using TimeTable.ViewModel.Commands;

namespace TimeTable.ViewModel
{
    public class DayViewModel : BaseViewModel
    {
        private readonly Day _dayData;
        private readonly int _parity;
        private readonly bool _isTeacher;
        private readonly int _holderId;
        private ObservableCollection<LessonViewModel> _lessons;
        private DateTime _date;

        public DayViewModel([NotNull] Day dayData, WeekType weekType, int parity,
            [NotNull] ICommandFactory commandFactory,
            University university, bool isTeacher, int holderId)
        {
            if (dayData == null) throw new ArgumentNullException("dayData");
            if (commandFactory == null) throw new ArgumentNullException("commandFactory");

            _dayData = dayData;
            _parity = parity;
            _isTeacher = isTeacher;
            _holderId = holderId;
            _date = SetUpDayName(weekType);

            if (_dayData.Lessons != null)
            {
                Lessons =
                    new ObservableCollection<LessonViewModel>(
                        FilterLessons(_dayData.Lessons, commandFactory, university));
            }
            else
            {
                Lessons = new ObservableCollection<LessonViewModel>();
            }
        }

        private IEnumerable<LessonViewModel> FilterLessons(IEnumerable<Lesson> lessons,
            ICommandFactory commandFactory, University university)
        {
            var ordered = lessons.OrderBy(lesson => lesson.TimeStart);
            foreach (var lesson in ordered)
            {
                if (lesson.Dates != null && lesson.Dates.Any())
                {
                    var formattedDay = _date.ToString("dd.MM.yyyy");
                    if (lesson.Dates.Contains(formattedDay))
                    {
                        yield return new LessonViewModel(lesson, commandFactory, university, _date, _isTeacher, _holderId);
                    }
                }
                else
                {
                    DateTime startDate;
                    DateTime endDate;
                    if (DateTime.TryParse(lesson.DateStart, out startDate) &&
                        DateTime.TryParse(lesson.DateEnd, out endDate))
                    {
                        if (_date < startDate || _date > endDate) continue;

                        if (IsVisibleInCurrentWeek(lesson))
                        {
                            yield return new LessonViewModel(lesson, commandFactory, university, _date, _isTeacher, _holderId);
                        }
                    }
                    else
                    {
                        if (IsVisibleInCurrentWeek(lesson))
                        {
                            yield return new LessonViewModel(lesson, commandFactory, university, _date, _isTeacher, _holderId);
                        }
                    }
                }
            }
        }

        private bool IsVisibleInCurrentWeek(Lesson lesson)
        {
            if (lesson.Parity == 0) return true;

            if (_parity == 0)
            {
                return lesson.Parity == 2;
            }
            return lesson.Parity == 1;
        }

        private DateTime SetUpDayName(WeekType weekType)
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
            return (monday + TimeSpan.FromDays(_dayData.Weekday - 1));
        }

        [UsedImplicitly(ImplicitUseKindFlags.Access)]
        public string NameForTheDay
        {
            get { return _date.ToString("d MMMM"); }
        }

        [NotNull]
        [UsedImplicitly(ImplicitUseKindFlags.Access)]
        public ObservableCollection<LessonViewModel> Lessons
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
            get { return _dayData.Weekday; }
        }
    }
}