using System;
using JetBrains.Annotations;

namespace TimeTable.Networking.Restful
{
    public abstract class RestfullRequest<T> where T : new()
    {
        private readonly TimeSpan _timeoutTimeSpan = TimeSpan.FromSeconds(40);
        private readonly string _baseUrl;
        private readonly WebService _webService;

        protected RestfullRequest([NotNull]string baseUrl, [NotNull] WebService webService)
        {
            if (baseUrl == null) throw new ArgumentNullException("baseUrl");
            if (webService == null) throw new ArgumentNullException("webService");
            
            _baseUrl = baseUrl;
            _webService = webService;
        }

        public string Url
        {
            get
            {
                return _baseUrl + AdditionalUrl;
            }
        }

        protected string AdditionalUrl { get; set; }

        public Type RequestType {get { return typeof (T); }}

        public IObservable<T> Execute()
        {
            return _webService.Get<T>(Url, _timeoutTimeSpan);
        }
    }
}
