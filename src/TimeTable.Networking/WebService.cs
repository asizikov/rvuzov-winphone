using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Text;
using JetBrains.Annotations;
using Newtonsoft.Json;
using RestSharp;
using TimeTable.Networking.Restful;

namespace TimeTable.Networking
{
    public class WebService
    {
        private readonly string _baseUrl;
        [NotNull] private readonly Deserializer _deserializer = new Deserializer();

        public WebService(string baseUrl)
        {
            _baseUrl = baseUrl;
        }

        public string BaseUrl { get { return _baseUrl; }}

        [NotNull] public IObservable<T> Get<T>(IRestRequest request, TimeSpan timeoutTimeSpan) where T : class 
        {
            return Observable.Create<T>(
                observer => 
                Scheduler.Default.Schedule(() =>
                {
                    var restClient = new RestClient(_baseUrl);
                    restClient.ExecuteAwait(request).ToObservable()
                        .Timeout(timeoutTimeSpan)
                        .Take(1)
                        .Subscribe(response => HandleResponce(response, observer),
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

        public IObservable<T> Post<T>(string url, string body, TimeSpan timeoutTimeSpan) where T : class 
        {
            return Observable.Create<T>(
               observer =>
               Scheduler.Default.Schedule(() =>
               {
                   var fullUrl = url;
                   var webRequest = (HttpWebRequest)WebRequest.Create(fullUrl);
                   webRequest.Method = "POST";
                   webRequest.ContentType = "application/json;charset=utf-8";

                   var fetchRequestStream = Observable.FromAsyncPattern<Stream>(webRequest.BeginGetRequestStream, webRequest.EndGetRequestStream);
                   var fetchResponse = Observable.FromAsyncPattern<WebResponse>(webRequest.BeginGetResponse, webRequest.EndGetResponse);

                   Func<Stream, IObservable<HttpWebResponse>> postDataAndFetchResponse = st =>
                   {
                       using (var writer = new StreamWriter(st))
                           writer.Write(body);
                       var temp = fetchResponse().Select(
                           rp => (HttpWebResponse)rp);
                       return temp;
                   };

                   Func<HttpWebResponse, IObservable<HttpWebResponse>> fetchResult = rp =>
                   {
                       if (rp.StatusCode == HttpStatusCode.OK)
                       {
                           return Observable.Return(rp);
                       }
                       var msg = "HttpStatusCode == " + rp.StatusCode.ToString();
                       var ex = new WebException(msg);
                       return Observable.Throw<HttpWebResponse>(ex);
                   };

                   var postResponse =
                       from st in fetchRequestStream()
                       from rp in postDataAndFetchResponse(st)
                       from s in fetchResult(rp)
                       select s;


                   postResponse
                       .Timeout(timeoutTimeSpan)
                       .Take(1)
                       .Subscribe(response => HandleResponce(response, observer),
                           ex =>
                           {
                               HandleException(ex);
                               observer.OnError(ex);
                               observer.OnCompleted();
                           },
                           observer.OnCompleted);
               }
               ));
        }

        private void HandleResponce<T>(IRestResponse response, IObserver<T> observer) where T : class
        {
            if (response.ResponseStatus == ResponseStatus.Error)
            {
                observer.OnError(new WebException(string.Format("failed to get response from {0}",response.ResponseUri)));
                return;
            }
            var json = response.Content;
            Debug.WriteLine(json);
            try
            {
                var result = _deserializer.Deserialize<T>(json);
                if (result != null)
                {
                    observer.OnNext(result);
                }
                else
                {
                    observer.OnError(new JsonSerializationException("Can't deserialize the responce : " + json));
                }
            }
            catch (JsonSerializationException exception)
            {
                observer.OnError(exception);
            }
        }


        private void HandleResponce<T>(WebResponse response, IObserver<T> observer) where T: class 
        {
            string json;
            using (var stream = response.GetResponseStream())
            {
                var reader = new StreamReader(stream, Encoding.UTF8);
                json = reader.ReadToEnd();
            }
            Debug.WriteLine(json);
            try
            {
                var result = _deserializer.Deserialize<T>(json);
                if (result != null)
                {
                    observer.OnNext(result);
                }
                else
                {
                    observer.OnError(new JsonSerializationException("Can't deserialize the responce : " + json));
                }
            }
            catch (JsonSerializationException exception)
            {
                observer.OnError(exception);
            }
        }

        private static void HandleException(Exception ignored)
        {
            if (ignored is WebException)
            {
                var ve = ignored as WebException;
                if (ve.Response != null)
                {
                    Debug.WriteLine("WebClient::exception::uir: " + ve.Response.ResponseUri);
                }
            }
            Debug.WriteLine("WebClient::exception: " + ignored);
        }
    }
}
