using System;

namespace TimeTable.Networking.Restful
{
    public abstract class RestfullRequest<T>
    {
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
    }
}
