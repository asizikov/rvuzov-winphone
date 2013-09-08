using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using TimeTable.Model;
using TimeTable.ViewModel.Commands;

namespace TimeTable.ViewModel
{
    public class TimeTableItemViewModel : BaseViewModel
    {
        private readonly Lesson _lesson;
        private readonly ICommandFactory _commandFactory;
        private readonly University _university;

        public TimeTableItemViewModel([NotNull] Lesson lesson, [NotNull] ICommandFactory commandFactory,
            [NotNull] University university)
        {
            if (lesson == null) throw new ArgumentNullException("lesson");
            if (commandFactory == null) throw new ArgumentNullException("commandFactory");
            if (university == null) throw new ArgumentNullException("university");
            _lesson = lesson;
            _commandFactory = commandFactory;
            _university = university;
        }


        [NotNull]
        public Lesson Lesson
        {
            get { return _lesson; }
        }

        [UsedImplicitly(ImplicitUseKindFlags.Access)]
        public string Auditory
        {
            get { return _lesson.Auditory; }
        }

        [UsedImplicitly(ImplicitUseKindFlags.Access)]
        public string Teachers
        {
            get { return FormatTeachersList(); }
        }

        [UsedImplicitly(ImplicitUseKindFlags.Access)]
        public string Groups
        {
            get { return FormatGroupsList(); }
        }

        private string FormatGroupsList()
        {
            if (_lesson.Groups == null || _lesson.Groups.Count == 0)
            {
                return null;
            }
            var sb = new StringBuilder();

            for (var index = 0; index < _lesson.Groups.Count; index++)
            {
                sb.Append(_lesson.Groups[index].GroupName);
                if (index != _lesson.Groups.Count - 1)
                {
                    sb.Append(",");
                }
            }
            return sb.ToString();
        }

        private string FormatTeachersList()
        {
            if (_lesson.Teachers == null || _lesson.Teachers.Count == 0)
            {
                return null;
            }
            var sb = new StringBuilder();

            for (var index = 0; index < _lesson.Teachers.Count; index++)
            {
                sb.Append(_lesson.Teachers[index].Name);
                if (index != _lesson.Teachers.Count - 1)
                {
                    sb.Append(",");
                }
            }
            return sb.ToString();
        }

        [UsedImplicitly(ImplicitUseKindFlags.Access)]
        public IEnumerable<AbstractMenuItem> ContextMenuItems
        {
            get
            {
                yield return new AbstractMenuItem 
                {
                    Command = null,
                    Header = "аудитория"

                };
                if (_lesson.Teachers != null && _lesson.Teachers.Any())
                {
                    yield return new AbstractMenuItem
                    {
                        CommandParameter = _lesson.Teachers.First().Id,
                        Command = _commandFactory.GetShowTeachersTimeTableCommand(_university,_lesson.Teachers.First()),
                        Header = "расписание преподавателя"
                    };
                }
                
            }
        }
    }
}