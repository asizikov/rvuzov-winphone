﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using TimeTable.Model;
using TimeTable.ViewModel.MenuItems;

namespace TimeTable.ViewModel
{
    public sealed class LessonViewModel : BaseViewModel
    {
        private readonly Lesson _lesson;
        private readonly DateTime _date;
        private readonly bool _isTeacher;
        private readonly int _holderId;
        private string _auditoriesList;
        private string _teachersList;
        private readonly LessonMenuItemsFactory _menuItemsFactory;

        public LessonViewModel([NotNull] Lesson lesson, [NotNull] LessonMenuItemsFactory menuItemsFactory, DateTime date,
            bool isTeacher, int holderId)
        {
            _lesson = lesson;
            _date = date;
            _isTeacher = isTeacher;
            _holderId = holderId;
            _menuItemsFactory = menuItemsFactory;
        }


        [NotNull]
        [UsedImplicitly(ImplicitUseKindFlags.Access)]
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
                    yield return _menuItemsFactory.CreateForAuditoriums(_lesson);
                }

                if (_lesson.Teachers != null && _lesson.Teachers.Any(t => t.Id != null))
                {
                    yield return _menuItemsFactory.CreateForTeachers(_lesson);
                }

                if (_lesson.Groups != null && _lesson.Groups.Any())
                {
                    yield return _menuItemsFactory.CreateForGroups(_lesson);
                }

                yield return _menuItemsFactory.CreateReportError(_holderId, _lesson.Id, _isTeacher);
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