using System;
using System.Net;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using TimeTable.Networking.Restful;

namespace TimeTable.Networking
{
    public class WebService
    {
        private readonly TimeSpan timeoutTimeSpan = TimeSpan.FromSeconds(40);

        public WebService()
        {
            
        }

        public IObservable<T> Get<T>(RestfullRequest<T> request ) where T : new()
        {
            return Observable.Create<T>(
                observer => 
                Scheduler.Default.Schedule(() =>
                {
                    var fullUrl = request.Url;
                    var webRequest = (HttpWebRequest)WebRequest.Create(fullUrl);
                    webRequest.Method = "GET";
                    Observable.FromAsyncPattern<WebResponse>(webRequest.BeginGetResponse, webRequest.EndGetResponse)()
                        .Timeout(timeoutTimeSpan)
                        .Subscribe(responce => HandleResponce(responce, request, observer),
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

        private void HandleResponce<T>(WebResponse responce, RestfullRequest<T> request , IObserver<T> observer) where T: new()
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
