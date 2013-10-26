using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using TimeTable.Model;
using TimeTable.ViewModel.Commands;

namespace TimeTable.ViewModel.Factories
{
    public class DayViewModelFactory
    {
        private readonly University _university;
        private readonly ICommandFactory _commandFactory;
        private readonly bool _isTeacher;
        private readonly int _holderId;

        public DayViewModelFactory([NotNull] University university, [NotNull] ICommandFactory commandFactory,
            bool isTeacher, int holderId)
        {
            if (university == null) throw new ArgumentNullException("university");
            if (commandFactory == null) throw new ArgumentNullException("commandFactory");
            _university = university;
            _commandFactory = commandFactory;
            _isTeacher = isTeacher;
            _holderId = holderId;
        }

        public IEnumerable<DayViewModel> CreateList(IEnumerable<Day> models, WeekType type, int parity)
        {
            var list =
                models.Select(
                    day => new DayViewModel(day, type, parity, _commandFactory, _university, _isTeacher, _holderId));
            return list;
        }
    }
}