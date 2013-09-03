﻿using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text;
using JetBrains.Annotations;

namespace TimeTable.Networking
{
    public class WebService
    {
        [NotNull] private readonly Deserializer _deserializer = new Deserializer();


        [NotNull] public IObservable<T> Get<T>(string url, TimeSpan timeoutTimeSpan) where T : new()
        {
            return Observable.Create<T>(
                observer => 
                Scheduler.Default.Schedule(() =>
                {
                    var fullUrl = url;
                    var webRequest = (HttpWebRequest)WebRequest.Create(fullUrl);
                    webRequest.Method = "GET";
                    Observable.FromAsyncPattern<WebResponse>(webRequest.BeginGetResponse, webRequest.EndGetResponse)()
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

        private void HandleResponce<T>(WebResponse response, IObserver<T> observer) where T: new()
        {
            string json;
            using (Stream stream = response.GetResponseStream())
            {
                var reader = new StreamReader(stream, Encoding.UTF8);
                json = reader.ReadToEnd();
            }
            Debug.WriteLine(json);
            var result = _deserializer.Deserialize<T>(json);
            observer.OnNext(result);
        }

// ReSharper disable once UnusedParameter.Local
        private static void HandleException(Exception ignored)
        {
        }


    }
}
