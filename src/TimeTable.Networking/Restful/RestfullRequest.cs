using System;
using JetBrains.Annotations;
using RestSharp;

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
        private readonly WebService _webService;

        protected RestfullRequest([NotNull] WebService webService)
        {
            if (webService == null) throw new ArgumentNullException("webService");
            _webService = webService;
        }

        public string Url
        {
            get { return _webService.BaseUrl + ResourceUrl; }
        }

        protected string ResourceUrl { get; set; }

        protected RequestMethod Method { get; set; }

        protected string Body { get; set; }

        public IObservable<T> Execute()
        {
            switch (Method)
            {
                case RequestMethod.Get:
                    return _webService.Get<T>(new RestRequest(ResourceUrl, RestSharp.Method.GET), _timeoutTimeSpan);
                case RequestMethod.Post:
                    return _webService.Post<T>(Url, Body, _timeoutTimeSpan);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}