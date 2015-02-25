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

    public static class RestfullRequest
    {
        public static RestfullRequest<T> Create<T>(string resource, [NotNull] WebService webService) where T: class
        {
            return new RestfullRequest<T>(webService, resource);
        }
    }

    public class RestfullRequest<T> where T : class
    {
        private readonly TimeSpan _timeoutTimeSpan = TimeSpan.FromSeconds(40);
        private readonly WebService _webService;

        public RestfullRequest([NotNull] WebService webService, [NotNull] string resourceUrl)
        {
            if (webService == null) throw new ArgumentNullException("webService");
            if (resourceUrl == null) throw new ArgumentNullException("resourceUrl");
            _webService = webService;
            ResourceUrl = resourceUrl;
        }

        public string Url
        {
            get { return _webService.BaseUrl + ResourceUrl; }
        }

        private string ResourceUrl { get; set; }

        private RequestMethod Method { get; set; }

        private string Body { get; set; }

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