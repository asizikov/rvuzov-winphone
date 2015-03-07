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

            PublishEvent(FlurryEvents.ChooseUniversity, parameters);
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
            PublishEvent(FlurryEvents.ChooseFaculty, parameters);
        }

        public void PublishGroupSelected([NotNull] Group selectedGroup, [NotNull] Faculty faculty, [NotNull] University university)
        {
            if (selectedGroup == null) throw new ArgumentNullException("selectedGroup");
            if (faculty == null) throw new ArgumentNullException("faculty");
            if (university == null) throw new ArgumentNullException("university");

            var parameters = new[]
            {
                new EventParameter("University_name", university.Name),
                new EventParameter("University_id", university.Id.ToString(CultureInfo.InvariantCulture)),
                new EventParameter("Faculty_id", faculty.Id.ToString(CultureInfo.InvariantCulture)),
                new EventParameter("Faculty_name", faculty.Title),
                new EventParameter("Group_name", selectedGroup.GroupName),
                new EventParameter("Group_id", selectedGroup.Id.ToString(CultureInfo.InvariantCulture))
 
            };
            PublishEvent(FlurryEvents.ChooseGroup, parameters);
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

            PublishEvent(FlurryEvents.ContextTeacherSchedule, parameters);
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

            PublishEvent(FlurryEvents.ActionbarScheduleSettings, parameters);
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

            PublishEvent(FlurryEvents.ActionbarToday, parameters);
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
            PublishEvent(FlurryEvents.ActionbarMarkFavorite, parameters);
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
            PublishEvent(FlurryEvents.ChooseTeacher, parameters);
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
            PublishEvent(FlurryEvents.ContextGroupSchedule,parameters);
        }

        public void PublishTimtableNotFoundEvent([CanBeNull] NavigationFlow navigationFlow)
        {
            if (navigationFlow == null)
            {
                PublishEvent(FlurryEvents.TimetableNotFound);
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
            PublishEvent(FlurryEvents.TimetableNotFound);
        }

        public void PublishPageLoadedUniversities()
        {
            PublishEvent(FlurryEvents.UniversitiesPageLoaded);
        }

        public void PublishPageLoadedSelectRole()
        {
            PublishEvent(FlurryEvents.SelectRolePageLoaded);
        }

        public void PublishPageLoadedGroups()
        {
            PublishEvent(FlurryEvents.SelectGroupsPageLoaded);
        }

        public void PublishPageLoadedFaculties()
        {
            PublishEvent(FlurryEvents.SelectUniversityPageLoaded);
        }

        public void PublishPageLoadedLessons()
        {
            PublishEvent(FlurryEvents.LessonsPageLoaded);
        }

        public void PublishPageLoadedFavorites()
        {
            PublishEvent(FlurryEvents.FavoritesPageLoaded);
        }

        public void PublishPageLoadedSettings()
        {
            PublishEvent(FlurryEvents.SettingsPageLoaded);
        }

        public void PublishPageLoadedAbout()
        {
            PublishEvent(FlurryEvents.AboutPageLoaded);
        }

        public void PublishShowMobile()
        {
            PublishEvent(FlurryEvents.SupportGoToMobileSite);
        }

        public void PublishUpdateLessonEvent()
        {
            PublishEvent(FlurryEvents.ContextEditEvent);
        }
    }
}