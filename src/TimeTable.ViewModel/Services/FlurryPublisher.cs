using System;
using System.Collections.Generic;
using System.Globalization;
using TimeTable.Model;

namespace TimeTable.ViewModel.Services
{
    public abstract class FlurryPublisher
    {
        private readonly List<FlurryEventWrapper> _eventQueue = new List<FlurryEventWrapper>();
        protected bool IsSessionActive;
        private readonly object _lockObject = new object();


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


        public void PublishUniversitySelected(University university)
        {
            var parameters = new[]
            {
                new EventParameter("University name", university.Name),
                new EventParameter("University shortname", university.ShortName),
                new EventParameter("University id", university.Id.ToString(CultureInfo.InvariantCulture)),
            };

            PublishEvent(FlurryEvents.EVENT_CHOOSE_UNIVERSITY, parameters);
        }
    }
}
