using System;

namespace TimeTable.Networking.Restful
{
    public abstract class RestfullRequest<T>
    {
        private readonly TimeSpan timeoutTimeSpan = TimeSpan.FromSeconds(40);
        private readonly string baseUrl;
        protected abstract string AdditionalUrl { get; set; }

        protected  RestfullRequest(string baseUrl)
        {
            if (baseUrl == null) throw new ArgumentNullException("baseUrl");
            this.baseUrl = baseUrl;
        }

        public string Url
        {
            get
            {
                return baseUrl + AdditionalUrl;
            } 
        }

        public TimeSpan Timeout {get { return timeoutTimeSpan; }}
    }
}
