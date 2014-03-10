using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using JetBrains.Annotations;
using TimeTable.Model;
using TimeTable.ViewModel.MenuItems;

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
            [NotNull] LessonMenuItemsFactory menuItemsFactory,
            bool isTeacher, int holderId)
        {
            if (dayData == null) throw new ArgumentNullException("dayData");
            if (menuItemsFactory == null) throw new ArgumentNullException("menuItemsFactory");

            _dayData = dayData;
            _parity = parity;
            _isTeacher = isTeacher;
            _holderId = holderId;
            _date = SetUpDayName(weekType);

            if (_dayData.Lessons != null)
            {
                Lessons =
                    new ObservableCollection<LessonViewModel>(
                        FilterLessons(_dayData.Lessons, menuItemsFactory));
            }
            else
            {
                Lessons = new ObservableCollection<LessonViewModel>();
            }
        }

        private IEnumerable<LessonViewModel> FilterLessons(IEnumerable<Lesson> lessons,
            LessonMenuItemsFactory menuItemsFactory)
        {
            var ordered = lessons.OrderBy(lesson => lesson.TimeStart);
            foreach (var lesson in ordered)
            {
                if (lesson.Dates != null && lesson.Dates.Any())
                {
                    var formattedDay = _date.ToString("dd.MM.yyyy");
                    if (lesson.Dates.Contains(formattedDay))
                    {
                        yield return
                            CreateLessonViewModel(menuItemsFactory, lesson);
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
                            yield return CreateLessonViewModel(menuItemsFactory, lesson);
                        }
                    }
                    else
                    {
                        if (IsVisibleInCurrentWeek(lesson))
                        {
                            yield return CreateLessonViewModel(menuItemsFactory, lesson);
                        }
                    }
                }
            }
        }

        private LessonViewModel CreateLessonViewModel(LessonMenuItemsFactory menuItemsFactory,
            Lesson lesson)
        {
            return new LessonViewModel(lesson, menuItemsFactory, _date, _isTeacher, _holderId);
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
            var delta = (7 + (today.DayOfWeek - DayOfWeek.Monday)) % 7;
            var monday = today.AddDays(-delta);

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
            return (monday + TimeSpan.FromDays(_dayData.Weekday - 1)).Date;
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