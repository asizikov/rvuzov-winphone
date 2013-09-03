using System;
using System.Linq;
using System.Text;
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
    }
}