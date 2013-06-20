using System;
using System.IO;
using System.Net;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text;
using TimeTable.Networking.Restful;

namespace TimeTable.Networking
{
    public class WebService
    {
        private readonly Deserializer deserializer = new Deserializer();


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
                        .Timeout(request.Timeout)
                        .Subscribe(response => HandleResponce(response, request, observer),
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

        private void HandleResponce<T>(WebResponse response, RestfullRequest<T> request , IObserver<T> observer) where T: new()
        {
            string json;
            using (Stream stream = response.GetResponseStream())
            {
                var reader = new StreamReader(stream, Encoding.UTF8);
                json = reader.ReadToEnd();
            }
            
            var result = deserializer.Deserialize<T>(json);
            observer.OnNext(result);
        }

        private void HandleException(Exception exception)
        {
            ;
        }
    }
}
