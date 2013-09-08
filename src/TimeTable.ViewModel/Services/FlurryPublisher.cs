using System;
using System.Collections.Generic;
using System.Globalization;
using JetBrains.Annotations;
using TimeTable.Model;

namespace TimeTable.ViewModel.Services
{
    public abstract class FlurryPublisher
    {
        [NotNull] private readonly List<FlurryEventWrapper> _eventQueue = new List<FlurryEventWrapper>();
        [NotNull] private readonly object _lockObject = new object();

        protected bool IsSessionActive;

        public void StartSession(string userId)
        {
            InitSession(userId);

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
        protected abstract void InitSession(string userId);
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
    }
}
