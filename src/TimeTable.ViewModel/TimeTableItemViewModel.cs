using System;
using JetBrains.Annotations;
using TimeTable.Model;

namespace TimeTable.ViewModel
{
    public class TimeTableItemViewModel : BaseViewModel
    {
        private readonly Lesson _lesson;

        public TimeTableItemViewModel([NotNull] Lesson lesson)
        {
            if (lesson == null) throw new ArgumentNullException("lesson");
            _lesson = lesson;
        }


        [NotNull]
        public Lesson Lesson
        {
            get { return _lesson; }
        }
    }
}