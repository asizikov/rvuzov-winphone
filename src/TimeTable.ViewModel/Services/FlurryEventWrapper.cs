using System;

namespace TimeTable.ViewModel.Services
{
    public class FlurryEventWrapper
    {

        public FlurryEventWrapper(Exception exception)
        {
            _exception = exception;
        }

        public FlurryEventWrapper(string eName, EventParameter[] param)
        {
            EventName = eName;
            Parameters = param;
        }

        private readonly Exception _exception;
        public Exception Exception
        {
            get { return _exception; }
        }

        public bool IsException
        {
            get { return _exception != null; }
        }

        public string EventName { get; private set; }

        public EventParameter[] Parameters { get; private set; }
    }
}