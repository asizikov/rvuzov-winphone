﻿using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using TimeTable.Model;
using TimeTable.ViewModel.MenuItems;

namespace TimeTable.ViewModel.Factories
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

        public IEnumerable<DayViewModel> CreateList(IEnumerable<Day> models, WeekType type, int parity)
        {
            return models.Select(
                day => new DayViewModel(day, type, parity, _menuItemsFactory, _isTeacher, _holderId));
        }
    }
}