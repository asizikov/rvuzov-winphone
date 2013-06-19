using System;
using System.Net;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace TimeTable.Networking
{
    public class WebService
    {
        private const string URL_PREFIX = "http://raspisanie-vuzov.ru/api/v1/";
        private readonly TimeSpan timeoutTimeSpan = TimeSpan.FromSeconds(40);

        public WebService()
        {
            
        }

        public IObservable<T> Get<T>(string url) where T : new()
        {
            return Observable.Create<T>(
                observer => 
                Scheduler.Default.Schedule(() =>
                {
                    var fullUrl = ComposeUrl(url);
                    var request = (HttpWebRequest)WebRequest.Create(fullUrl);
                    request.Method = "GET";
                    Observable.FromAsyncPattern<WebResponse>(request.BeginGetResponse, request.EndGetResponse)()
                        .Timeout(timeoutTimeSpan)
                        .Subscribe(responce => HandleResponce(responce, url, observer),
                            ex =>
                            {
                                HandleException(ex);
                                observer.OnError(ex);
                                observer.OnCompleted();
                            },
                            observer.OnCompleted );
                }
                ));
        }

        private string ComposeUrl(string url)
        {
            return URL_PREFIX + url;
        }

        private void HandleResponce<T>(WebResponse responce, string url, IObserver<T> observer) where T: new()
        {
            var result = new T();
            observer.OnNext(result);
        }

        private void HandleException(Exception exception)
        {
            ;
        }
    }
}
