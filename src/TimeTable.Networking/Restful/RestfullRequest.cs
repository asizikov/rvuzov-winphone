using System;
using JetBrains.Annotations;

namespace TimeTable.Networking.Restful
{
    public enum RequestMethod
    {
        Get = 0,
        Post = 1
    }

    public abstract class RestfullRequest<T> where T : class
    {
        private readonly TimeSpan _timeoutTimeSpan = TimeSpan.FromSeconds(40);
        private readonly string _baseUrl;
        private readonly WebService _webService;

        protected RestfullRequest([NotNull] string baseUrl, [NotNull] WebService webService)
        {
            if (baseUrl == null) throw new ArgumentNullException("baseUrl");
            if (webService == null) throw new ArgumentNullException("webService");

            _baseUrl = baseUrl;
            _webService = webService;
        }

        public string Url
        {
            get { return _baseUrl + AdditionalUrl; }
        }

        protected string AdditionalUrl { get; set; }

        public RequestMethod Method { get; protected set; }

        protected string Body { get; set; }

        public IObservable<T> Execute()
        {
            switch (Method)
            {
                case RequestMethod.Get:
                    return _webService.Get<T>(Url, _timeoutTimeSpan);
                case RequestMethod.Post:
                    return _webService.Post<T>(Url, Body, _timeoutTimeSpan);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}