using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using TimeTable.Domain.Lessons;

namespace TimeTable.ViewModel.WeekOverview.Factories
{
    internal class WeekViewModelFactory
    {
        private readonly DayViewModelFactory _dayViewModelFactory;

        public WeekViewModelFactory([NotNull] LessonMenuItemsFactory menuItemsFactory, bool isTeacher, int id)
        {
            if (menuItemsFactory == null) throw new ArgumentNullException("menuItemsFactory");

            _dayViewModelFactory = new DayViewModelFactory(menuItemsFactory, isTeacher, id);
        }

        public WeekViewModel Create([CanBeNull] IEnumerable<Day> days, int weekNumber, WeekType weekType)
        {
            return new WeekViewModel(days, weekNumber, _dayViewModelFactory, weekType);
        }
    }
}