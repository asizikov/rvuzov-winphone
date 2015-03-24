using System;
using System.Diagnostics;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using JetBrains.Annotations;
using TimeTable.Domain.Lessons;
using TimeTable.Networking.Cache;
using TimeTable.Networking.Restful;

namespace TimeTable.Data
{
    public abstract class BaseAsyncWebClient
    {
        private readonly IWebCache _cache;
        private RestfulCallFactory CallFactory { get; set; }

        protected BaseAsyncWebClient([NotNull] IWebCache cache)
        {
            if (cache == null) throw new ArgumentNullException("cache");
            CallFactory = new RestfulCallFactory();
            _cache = cache;
        }

        protected IObservable<T> GetDataAsync<T>(RestfullRequest<T> request,
                                                 CachePolicy cachePolicy = CachePolicy.GetFromCacheAndUpdate)
            where T : class
        {
            if (!_cache.IsCached<T>(request.Url))
            {
                Debug.WriteLine("DataProvider::Getting data:" + request.Url);
                return Observable.Create<T>(
                    observer =>
                        Scheduler.Default.Schedule(() => ExecuteRequest(request, observer))
                    );
            }
            Debug.WriteLine("DataProvider::Getting cached data:" + request.Url);
            return Observable.Create<T>(
                observer =>
                    Scheduler.Default.Schedule(() =>
                    {
                        var item = _cache.Fetch<T>(request.Url);
                        observer.OnNext(item);
                        if (cachePolicy == CachePolicy.TryGetFromCache)
                        {
                            observer.OnCompleted();
                        }
                        else
                        {
                            var updatable = item as IUpdatableModel;
                            if (updatable != null)
                            {
                                CheckIfNeededToBeUpdated(request, updatable, observer);
                            }
                            else
                            {
                                Debug.WriteLine("UpdateData:" + request.Url);
                                ExecuteRequest(request, observer, true);
                            }
                        }
                    })
                );
        }


        private void ExecuteRequest<T>(RestfullRequest<T> request, IObserver<T> observer, bool ignoreErrors = false)
            where T : class
        {
            request.Execute()
                   .Subscribe(result =>
                   {
                       _cache.Put(result, request.Url);
                       observer.OnNext(result);
                   },
                       ex =>
                       {
                           if (!ignoreErrors)
                           {
                               observer.OnError(ex);
                           }
                       },
                       observer.OnCompleted);
        }

        private void CheckIfNeededToBeUpdated<T>(RestfullRequest<T> request, IUpdatableModel updatable,
                                                 IObserver<T> observer)
            where T : class
        {
            var lastUpdated = updatable.LastUpdated;
            var lastUpdatedRequest = CallFactory.GetLastUpdatedRequest(request.ResourceUrl);
            Debug.WriteLine("WebClient::CheckIfNeededToBeUpdated " + lastUpdatedRequest.Url);
            lastUpdatedRequest.Execute()
                              .Subscribe(
                                  resutl =>
                                  {
                                      if (resutl.Data > lastUpdated)
                                      {
                                          Debug.WriteLine("WebClient::CheckIfNeededToBeUpdated::Updating " + request.Url);
                                          ExecuteRequest(request, observer);
                                      }
                                      else
                                      {
                                          observer.OnCompleted();
                                      }
                                  }, ex => observer.OnCompleted());
        }
    }
}