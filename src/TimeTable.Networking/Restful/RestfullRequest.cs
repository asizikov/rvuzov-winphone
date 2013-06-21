using System;
using JetBrains.Annotations;

namespace TimeTable.Networking.Restful
{
    public abstract class RestfullRequest<T> where T : new()
    {
        private readonly TimeSpan timeoutTimeSpan = TimeSpan.FromSeconds(40);
        private readonly string baseUrl;
        private readonly WebService webService;

        protected RestfullRequest([NotNull]string baseUrl, [NotNull] WebService webService)
        {
            if (baseUrl == null) throw new ArgumentNullException("baseUrl");
            if (webService == null) throw new ArgumentNullException("webService");
            this.baseUrl = baseUrl;
            this.webService = webService;
        }

        public string Url
        {
            get
            {
                return baseUrl + AdditionalUrl;
            }
        }

        protected abstract string AdditionalUrl { get; set; }
        public Type RequestType {get { return typeof (T); }}

        public IObservable<T> Execute()
        {
            return webService.Get<T>(Url, timeoutTimeSpan);
        }
    }
}
