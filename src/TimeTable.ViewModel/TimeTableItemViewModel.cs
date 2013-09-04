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

        public TimeTableItemViewModel([NotNull] Lesson lesson, [NotNull] ICommandFactory commandFactory)
        {
            if (lesson == null) throw new ArgumentNullException("lesson");
            if (commandFactory == null) throw new ArgumentNullException("commandFactory");
            _lesson = lesson;
            _commandFactory = commandFactory;
        }


        [NotNull]
        public Lesson Lesson
        {
            get { return _lesson; }
        }

        public string Auditory
        {
            get { return _lesson.Auditory; }
        }

        public string Teachers
        {
            get { return FormatTeachersList(); }
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
                        Command = _commandFactory.GetShowTeachersTimeTableCommand(),
                        Header = "расписание преподавателя"
                    };
                }
                
            }
        }
    }
}