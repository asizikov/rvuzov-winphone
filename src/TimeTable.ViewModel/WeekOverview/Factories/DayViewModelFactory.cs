using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using TimeTable.Domain.Lessons;
using TimeTable.Domain.Participants;
using TimeTable.ViewModel.OrganizationalStructure;

namespace TimeTable.ViewModel.WeekOverview.Factories
{
    public class DayViewModelFactory
    {
        private readonly LessonMenuItemsFactory _menuItemsFactory;
        private readonly bool _isTeacher;
        private readonly int _holderId;

        public DayViewModelFactory([NotNull] LessonMenuItemsFactory menuItemsFactory, bool isTeacher, int holderId)
        {
            if (menuItemsFactory == null) throw new ArgumentNullException("menuItemsFactory");
            _menuItemsFactory = menuItemsFactory;
            _isTeacher = isTeacher;
            _holderId = holderId;
        }

        public IEnumerable<DayViewModel> CreateList([CanBeNull] IEnumerable<Day> models, NavigationFlow navigationFlow,
                                                    Group group, WeekType type, int parity)
        {
            if (models == null)
            {
                return Enumerable.Empty<DayViewModel>();
            }
            return models.Select(
                day =>
                    new DayViewModel(day, type, parity, _menuItemsFactory, _isTeacher, _holderId, navigationFlow, group));
        }
    }
}