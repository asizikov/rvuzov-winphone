using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using TimeTable.Model;
using TimeTable.ViewModel.Commands;

namespace TimeTable.ViewModel
{
    public sealed class LessonViewModel : BaseViewModel
    {
        private readonly Lesson _lesson;
        private readonly ICommandFactory _commandFactory;
        private readonly University _university;
        private readonly OptionsMonitor _optionsMonitor;
        private readonly DateTime _date;
        private readonly bool _isTeacher;
        private readonly int _holderId;
        private string _auditoriesList;
        private string _teachersList;

        public LessonViewModel([NotNull] Lesson lesson, [NotNull] ICommandFactory commandFactory,
            [NotNull] University university, OptionsMonitor optionsMonitor, DateTime date, bool isTeacher, int holderId)
        {
            if (lesson == null) throw new ArgumentNullException("lesson");
            if (commandFactory == null) throw new ArgumentNullException("commandFactory");
            if (university == null) throw new ArgumentNullException("university");
            _lesson = lesson;
            _commandFactory = commandFactory;
            _university = university;
            _optionsMonitor = optionsMonitor;
            _date = date;
            _isTeacher = isTeacher;
            _holderId = holderId;
        }


        [NotNull]
        public Lesson Lesson
        {
            get { return _lesson; }
        }

        [UsedImplicitly(ImplicitUseKindFlags.Access)]
        public string Auditory
        {
            get { return FormatAuditories(); }
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

        [UsedImplicitly(ImplicitUseKindFlags.Access)]
        public bool IsCurrent
        {
            get { return IsLessonCurrent(); }
        }

        [CanBeNull]
        private string FormatGroupsList()
        {
            if (_lesson.Groups == null || !_lesson.Groups.Any())
            {
                return null;
            }
            var sb = new StringBuilder();

            for (var index = 0; index < _lesson.Groups.Count; index++)
            {
                sb.Append(_lesson.Groups[index].GroupName);
                if (index != _lesson.Groups.Count - 1)
                {
                    sb.Append(", ");
                }
            }
            return sb.ToString();
        }

        [CanBeNull]
        private string FormatTeachersList()
        {
            if (_teachersList != null)
            {
                return _teachersList;
            }

            if (_lesson.Teachers == null || !_lesson.Teachers.Any())
            {
                return null;
            }

            var sb = new StringBuilder();

            for (var index = 0; index < _lesson.Teachers.Count; index++)
            {
                sb.Append(_lesson.Teachers[index].Name);
                if (index != _lesson.Teachers.Count - 1)
                {
                    sb.Append(", ");
                }
            }
            _teachersList = sb.ToString();
            return _teachersList;
        }

        [CanBeNull]
        private string FormatAuditories()
        {
            if (_auditoriesList != null)
            {
                return _auditoriesList;
            }
            if (_lesson.Auditoriums == null || !_lesson.Auditoriums.Any())
            {
                return null;
            }

            var sb = new StringBuilder();
            for (var index = 0; index < _lesson.Auditoriums.Count; index++)
            {
                var name = _lesson.Auditoriums[index].Name;
                if (!string.IsNullOrWhiteSpace(name))
                {
                    sb.Append(name);
                }
                if (index != _lesson.Auditoriums.Count - 1)
                {
                    sb.Append(", ");
                }
            }
            _auditoriesList = sb.ToString();
            return _auditoriesList;
        }

        [UsedImplicitly(ImplicitUseKindFlags.Access)]
        public IEnumerable<AbstractMenuItem> ContextMenuItems
        {
            get
            {
                if (_lesson.Auditoriums != null && _lesson.Auditoriums.Any())
                {
                    //TODO _lesson.Auditoriums.Single(), ибо контекстное меню мы можем выбрать только для одной конкретной пары (если оно вообще есть),
                    //TODO если же у нас несколько аудиторий для одной пары, то что-то не то (или я чего-то не понимаю?)
                    var auditoriumInfoCommand = _commandFactory.GetShowAuditoriumCommand(_lesson.Auditoriums.Single());
                    yield return new AbstractMenuItem
                    {
                        Command = auditoriumInfoCommand,
                        Header = auditoriumInfoCommand.Title
                    };
                }

                if (_lesson.Teachers != null && _lesson.Teachers.Any())
                {
                    var showTeachersTimeTableCommand = _commandFactory.GetShowTeachersTimeTableCommand(_university,
                        _lesson.Teachers.First());
                    yield return new AbstractMenuItem
                    {
                        Command = showTeachersTimeTableCommand,
                        Header = showTeachersTimeTableCommand.Title
                    };
                }

                if (_lesson.Groups != null && _lesson.Groups.Any())
                {
                    //_lesson.Groups.Add(_lesson.Groups.First());
                    if (_lesson.Groups.Count > 1)
                    {
                        var options = _lesson.Groups.Select(g => new OptionsItem
                        {
                            Title = g.GroupName,
                            Command = _commandFactory.GetShowGroupTimeTableCommand(_university,g)
                        });
                        yield return new AbstractMenuItem
                        {
                            Command = new SimpleCommand(() =>
                            {
                                _optionsMonitor.Items = new ObservableCollection<OptionsItem>(options);
                                _optionsMonitor.IsVisible = true;
                            }),
                            Header = options.First().Command.Title
                        };
                    }
                    else
                    {
                        var showTeachersTimeTableCommand = _commandFactory.GetShowGroupTimeTableCommand(_university,
                            _lesson.Groups.First());
                        yield return new AbstractMenuItem
                        {
                            Command = showTeachersTimeTableCommand,
                            Header = showTeachersTimeTableCommand.Title
                        };
                    }
                }

                var reportErrorCommand = _commandFactory.GetReportErrorCommand(_holderId, _lesson.Id, _isTeacher);
                yield return new AbstractMenuItem
                {
                    Command = reportErrorCommand,
                    Header = reportErrorCommand.Title
                };
            }
        }

        private bool IsLessonCurrent()
        {
            if (DateTime.Now.Day != _date.Day) return false;

            var now = DateTime.Now.ToString("HH:mm");
            var lessonStarted = string.Compare(now, _lesson.TimeStart, CultureInfo.InvariantCulture,
                CompareOptions.IgnoreCase);
            var lessonEnded = string.Compare(now, _lesson.TimeEnd, CultureInfo.InvariantCulture,
                CompareOptions.IgnoreCase);
            return lessonStarted >= 0 && lessonEnded <= 0;
        }
    }
}