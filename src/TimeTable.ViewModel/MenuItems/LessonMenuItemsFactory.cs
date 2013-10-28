using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using JetBrains.Annotations;
using TimeTable.Model;
using TimeTable.ViewModel.Commands;

namespace TimeTable.ViewModel.MenuItems
{
    public class LessonMenuItemsFactory
    {
        private readonly ICommandFactory _commandFactory;
        private readonly University _university;
        private readonly OptionsMonitor _optionsMonitor;

        public LessonMenuItemsFactory([NotNull] ICommandFactory commandFactory, [NotNull] University university,
            [NotNull] OptionsMonitor optionsMonitor)
        {
            if (commandFactory == null) throw new ArgumentNullException("commandFactory");
            if (university == null) throw new ArgumentNullException("university");
            if (optionsMonitor == null) throw new ArgumentNullException("optionsMonitor");
            _commandFactory = commandFactory;
            _university = university;
            _optionsMonitor = optionsMonitor;
        }

        public AbstractMenuItem CreateForGroups(Lesson lesson)
        {
            if (lesson.Groups.Count > 1)
            {
                var options = lesson.Groups.Select(g => new OptionsItem
                {
                    Title = g.GroupName,
                    Command = _commandFactory.GetShowGroupTimeTableCommand(_university, g),
                });
                var menuItem = FormatAbstractMenuItem(_optionsMonitor, options);
                return menuItem;
            }
            return CreateForOneGroup(lesson);
        }

        public AbstractMenuItem CreateForTeachers(Lesson lesson)
        {
            if (lesson.Teachers.Count <= 1) return CreateForOneTeacher(lesson);

            var options = lesson.Teachers.Where(t => !string.IsNullOrWhiteSpace(t.Id))
                .Select(t => new OptionsItem
                {
                    Title = t.Name,
                    Command = _commandFactory.GetShowTeachersTimeTableCommand(_university, t)
                });

            var menuItem = FormatAbstractMenuItem(_optionsMonitor, options);
            return menuItem;
        }

        public AbstractMenuItem CreateForAuditoriums(Lesson lesson)
        {
            if (lesson.Auditoriums.Count > 1)
            {
                var options = lesson.Auditoriums.Select(a => new OptionsItem
                {
                    Title = a.Name,
                    Command = _commandFactory.GetShowAuditoriumCommand(a, _university.Id)
                });
                return FormatAbstractMenuItem(_optionsMonitor, options);
            }
            return CreateForOneAuditorium(lesson);
        }

        private AbstractMenuItem CreateForOneGroup(Lesson lesson)
        {
            var command = _commandFactory.GetShowGroupTimeTableCommand(_university,
                lesson.Groups.First());
            var menuItem = new AbstractMenuItem
            {
                Command = command,
                Header = command.Title
            };
            return menuItem;
        }

        private AbstractMenuItem CreateForOneTeacher(Lesson lesson)
        {
            var showTeachersTimeTableCommand = _commandFactory.GetShowTeachersTimeTableCommand(_university,
                lesson.Teachers.First());
            return new AbstractMenuItem
            {
                Command = showTeachersTimeTableCommand,
                Header = showTeachersTimeTableCommand.Title
            };
        }

        private AbstractMenuItem CreateForOneAuditorium(Lesson lesson)
        {
            var auditoriumInfoCommand = _commandFactory.GetShowAuditoriumCommand(lesson.Auditoriums.Single(),
                _university.Id);
            return new AbstractMenuItem
            {
                Command = auditoriumInfoCommand,
                Header = auditoriumInfoCommand.Title
            };
        }

        public AbstractMenuItem CreateReportError(int holderId, int lessonId, bool isTeacher)
        {
            var reportErrorCommand = _commandFactory.GetReportErrorCommand(holderId, lessonId, isTeacher);
            return new AbstractMenuItem
            {
                Command = reportErrorCommand,
                Header = reportErrorCommand.Title
            };
        }

        [Pure, NotNull]
        private static AbstractMenuItem FormatAbstractMenuItem(OptionsMonitor optionsMonitor,
            IEnumerable<OptionsItem> options)
        {
            var menuItem = new AbstractMenuItem
            {
                Command = new SimpleCommand(() =>
                {
                    optionsMonitor.Items = new ObservableCollection<OptionsItem>(options);
                    optionsMonitor.IsVisible = true;
                    optionsMonitor.Title = optionsMonitor.Items.First().Command.Title;
                }),
                Header = options.First().Command.Title
            };
            return menuItem;
        }
    }
}