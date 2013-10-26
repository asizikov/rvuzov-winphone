using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using TimeTable.Model;
using TimeTable.ViewModel.Commands;

namespace TimeTable.ViewModel.Factories
{
    internal class WeekViewModelFactory
    {
        private readonly DayViewModelFactory _dayViewModelFactory;

        public WeekViewModelFactory([NotNull] ICommandFactory commandFactory, [NotNull] University university,
            bool isTeacher, int id)
        {
            if (commandFactory == null) throw new ArgumentNullException("commandFactory");
            if (university == null) throw new ArgumentNullException("university");
            _dayViewModelFactory = new DayViewModelFactory(university, commandFactory, isTeacher, id);
        }

        public WeekViewModel Create(IEnumerable<Day> days, int weekNumber, WeekType weekType)
        {
            return new WeekViewModel(days, weekNumber, _dayViewModelFactory, weekType);
        }
    }
}