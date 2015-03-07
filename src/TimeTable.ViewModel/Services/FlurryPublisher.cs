using System;
using System.Collections.Generic;
using System.Globalization;
using JetBrains.Annotations;
using TimeTable.Domain.OrganizationalStructure;
using TimeTable.Domain.Participants;
using TimeTable.ViewModel.OrganizationalStructure;

namespace TimeTable.ViewModel.Services
{
    public abstract class FlurryPublisher
    {
        [NotNull] private readonly List<FlurryEventWrapper> _eventQueue = new List<FlurryEventWrapper>();
        [NotNull] private readonly object _lockObject = new object();

        protected bool IsSessionActive;

        public void StartSession()
        {
            InitSession();

            if (IsSessionActive) throw new InvalidOperationException();
            IsSessionActive = true;

            FlurryEventWrapper[] copy;
            lock (_lockObject)
            {
                copy = _eventQueue.ToArray();
                _eventQueue.Clear();
            }
            if (copy.Length > 0)
            {
                FlushEvents(copy);
            }
        }

        public void PublishException(Exception exception)
        {
            if (!IsSessionActive)
            {
                lock (_lockObject)
                {
                    //// ReSharper disable ConditionIsAlwaysTrueOrFalse
                    //   This is not always true, because it can be set from other thread
                    if (!IsSessionActive)
                        //// ReSharper restore ConditionIsAlwaysTrueOrFalse
                    {
                        _eventQueue.Add(new FlurryEventWrapper(exception));
                        return;
                    }
                }
            }
            SendError(exception);
        }

        private void PublishEvent(string eventName, params EventParameter[] parameters)
        {
            if (!IsSessionActive)
            {
                lock (_lockObject)
                {
                    //// ReSharper disable ConditionIsAlwaysTrueOrFalse
                    //   This is not always true, because it can be set from other thread
                    if (!IsSessionActive)
                        //// ReSharper restore ConditionIsAlwaysTrueOrFalse
                    {
                        _eventQueue.Add(new FlurryEventWrapper(eventName, parameters));
                        return;
                    }
                }
            }
            FlushEvent(eventName, parameters);
        }

        public void EndSession()
        {
            if (!IsSessionActive) throw new InvalidOperationException();

            IsSessionActive = false;
            CloseSesstion();
        }


        private void FlushEvents(IEnumerable<FlurryEventWrapper> eventsCopy)
        {
            foreach (var eventItem in eventsCopy)
            {
                if (!eventItem.IsException)
                {
                    FlushEvent(eventItem.EventName, eventItem.Parameters);
                }
                else
                {
                    SendError(eventItem.Exception);
                }
            }
        }

        protected abstract void SendError(Exception exception);
        protected abstract void CloseSesstion();
        protected abstract void InitSession();
        protected abstract void FlushEvent(string eventName, EventParameter[] parameters);

        public void PublishUniversitySelected([NotNull] University university)
        {
            if (university == null) throw new ArgumentNullException("university");

            var parameters = new[]
            {
                new EventParameter("University name", university.Name),
                new EventParameter("University shortname", university.ShortName),
                new EventParameter("University id", university.Id.ToString(CultureInfo.InvariantCulture))
            };

            PublishEvent(FlurryEvents.EVENT_CHOOSE_UNIVERSITY, parameters);
        }

        public void PublishFacultySelected([NotNull] Faculty selectedFaculty, [NotNull] University university)
        {
            if (selectedFaculty == null) throw new ArgumentNullException("selectedFaculty");
            if (university == null) throw new ArgumentNullException("university");

            var parameters = new[]
            {
                new EventParameter("University name", university.Name),
                new EventParameter("University shortname", university.ShortName),
                new EventParameter("University id", university.Id.ToString(CultureInfo.InvariantCulture)),
                new EventParameter("Faculty name", selectedFaculty.Title),
                new EventParameter("Faculty id", selectedFaculty.Id.ToString(CultureInfo.InvariantCulture))
            };
            PublishEvent(FlurryEvents.EVENT_CHOOSE_FACULTY, parameters);
        }

        public void PublishGroupSelected([NotNull] Group selectedGroup, [NotNull] University university)
        {
            if (selectedGroup == null) throw new ArgumentNullException("selectedGroup");
            if (university == null) throw new ArgumentNullException("university");

            var parameters = new[]
            {
                new EventParameter("University name", university.Name),
                new EventParameter("University shortname", university.ShortName),
                new EventParameter("University id", university.Id.ToString(CultureInfo.InvariantCulture)),
                new EventParameter("Group name", selectedGroup.GroupName),
                new EventParameter("Group id", selectedGroup.Id.ToString(CultureInfo.InvariantCulture))
            };
            PublishEvent(FlurryEvents.EVENT_CHOOSE_GROUP, parameters);
        }

        public void PublishContextMenuShowTeachersTimeTable(University university, string name, string id)
        {
            var parameters = new[]
            {
                new EventParameter("University name", university.Name),
                new EventParameter("University shortname", university.ShortName),
                new EventParameter("University id", university.Id.ToString(CultureInfo.InvariantCulture)),
                new EventParameter("Object name", name),
                new EventParameter("Object Id", id),
                new EventParameter("Mode", "teacher")
            };

            PublishEvent(FlurryEvents.EVENT_CONTEXT_TEACHER_SCHEDULE, parameters);
        }

        public void PublishActionbarScheduleSettings([NotNull] University university, bool isTeacher, string name,
            int id)
        {
            var mode = "teacher";

            if (university == null) throw new ArgumentNullException("university");
            if (isTeacher == false) mode = "student";

            var parameters = new[]
            {
                new EventParameter("University shortname", university.ShortName),
                new EventParameter("University id", university.Id.ToString(CultureInfo.InvariantCulture)),
                new EventParameter("Object name", name),
                new EventParameter("Object Id", id.ToString(CultureInfo.InvariantCulture)),
                new EventParameter("Mode", mode)
            };

            PublishEvent(FlurryEvents.EVENT_ACTIONBAR_SCHEDULE_SETTINGS, parameters);
        }


        public void PublishActionbarToday([NotNull] University university, bool isTeacher, string name, int id)
        {
            var mode = "teacher";

            if (university == null) throw new ArgumentNullException("university");
            if (isTeacher == false) mode = "student";
            var parameters = new[]
            {
                new EventParameter("University shortname", university.ShortName),
                new EventParameter("University id", university.Id.ToString(CultureInfo.InvariantCulture)),
                new EventParameter("Object name", name),
                new EventParameter("Object Id", id.ToString(CultureInfo.InvariantCulture)),
                new EventParameter("Mode", mode)
            };

            PublishEvent(FlurryEvents.EVENT_ACTIONBAR_TODAY, parameters);
        }

        public void PublishMarkFavorite([NotNull] University university, bool isTeacher, string name, int id)
        {
            var mode = "teacher";
            if (university == null) throw new ArgumentNullException("university");
            if (isTeacher == false) mode = "student";


            var parameters = new[]
            {
                new EventParameter("University shortname", university.ShortName),
                new EventParameter("University id", university.Id.ToString(CultureInfo.InvariantCulture)),
                new EventParameter("Object name", name),
                new EventParameter("Object Id", id.ToString(CultureInfo.InvariantCulture)),
                new EventParameter("Mode", mode)
            };
            PublishEvent(FlurryEvents.EVENT_ACTIONBAR_MARK_FAVORITE, parameters);
        }

        public void PublishTeacherSelected([NotNull] Teacher selectedTeacher, [NotNull] University university)
        {
            if (selectedTeacher == null) throw new ArgumentNullException("selectedTeacher");
            if (university == null) throw new ArgumentNullException("university");

            var parameters = new[]
            {
                new EventParameter("University name", university.Name),
                new EventParameter("University shortname", university.ShortName),
                new EventParameter("University id", university.Id.ToString(CultureInfo.InvariantCulture)),
                new EventParameter("Teacher name", selectedTeacher.Name),
                new EventParameter("Teacher Id", selectedTeacher.Id.ToString(CultureInfo.InvariantCulture))
            };
            PublishEvent(FlurryEvents.EVENT_CHOOSE_TEACHER, parameters);
        }

        public void PublishContextMenuShowGroupTimeTable(University university, string groupName, int id)
        {
            var parameters = new[]
            {
                new EventParameter("University name", university.Name),
                new EventParameter("University shortname", university.ShortName),
                new EventParameter("University id", university.Id.ToString(CultureInfo.InvariantCulture)),
                new EventParameter("Group name", groupName),
                new EventParameter("Teacher Id", id.ToString(CultureInfo.InvariantCulture))
            };
            PublishEvent(FlurryEvents.EVENT_CONTEXT_GROUP_SCHEDULE,parameters);
        }

        public void PublishTimtableNotFoundEvent([CanBeNull] NavigationFlow navigationFlow)
        {
            if (navigationFlow == null)
            {
                PublishEvent(FlurryEvents.EVENT_TIMETABLE_NOT_FOUND);
                return;
            }
            var parameters = new List<EventParameter>
            {
                new EventParameter("University name", navigationFlow.UniversityName),
                new EventParameter("University id", navigationFlow.UniversityId.ToString(CultureInfo.InvariantCulture)),
            };

            if (navigationFlow.FacultyId != 0)
            {
                parameters.Add(new EventParameter("Faculty_id", navigationFlow.FacultyId.ToString(CultureInfo.InvariantCulture)));
                parameters.Add(new EventParameter("Faculty_name", navigationFlow.FacultyName));
            }
            PublishEvent(FlurryEvents.EVENT_TIMETABLE_NOT_FOUND);
        }

        public void PublishReportError(string message)
        {
            var parameters = new[]
            {
                new EventParameter("Error message", message)
            };
            PublishEvent(FlurryEvents.EVENT_CONTEXT_REPORT_ERROR, parameters);
        }

        public void PublishPageLoadedUniversities()
        {
            PublishEvent(FlurryEvents.EventUniversitiesPageLoaded);
        }

        public void PublishPageLoadedReportError()
        {
            PublishEvent(FlurryEvents.EventReportErrorPageLoaded);
        }

        public void PublishPageLoadedSelectRole()
        {
            PublishEvent(FlurryEvents.EventSelectRolePageLoaded);
        }

        public void PublishPageLoadedGroups()
        {
            PublishEvent(FlurryEvents.EventSelectGroupsPageLoaded);
        }

        public void PublishPageLoadedFaculties()
        {
            PublishEvent(FlurryEvents.EventSelectUniversityPageLoaded);
        }

        public void PublishPageLoadedLessons()
        {
            PublishEvent(FlurryEvents.EventLessonsPageLoaded);
        }

        public void PublishPageLoadedFavorites()
        {
            PublishEvent(FlurryEvents.EventFavoritesPageLoaded);
        }

        public void PublishPageLoadedSettings()
        {
            PublishEvent(FlurryEvents.EventSettingsPageLoaded);
        }

        public void PublishPageLoadedAbout()
        {
            PublishEvent(FlurryEvents.EventAboutPageLoaded);
        }

        public void PublishShowMobile()
        {
            PublishEvent(FlurryEvents.EVENT_SUPPORT_GO_TO_MOBILE_SITE);
        }


        public void PublishUpdateLessonEvent()
        {
            PublishEvent(FlurryEvents.EVENT_CONTEXT_EDIT_EVENT);
        }
    }
}