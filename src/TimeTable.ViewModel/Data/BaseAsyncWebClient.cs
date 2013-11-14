using System;
using System.Diagnostics;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using JetBrains.Annotations;
using TimeTable.Model;
using TimeTable.Networking.Restful;
using TimeTable.ViewModel.Restful;

namespace TimeTable.ViewModel.Data
{
    public enum CachePolicy
    {
        GetFromCacheAndUpdate,
        TryGetFromCache
    }

    public abstract class BaseAsyncWebClient
    {
        private readonly IWebCache _cache;
        protected readonly RestfulCallFactory CallFactory = new RestfulCallFactory();

        protected BaseAsyncWebClient([NotNull] IWebCache cache)
        {
            if (cache == null) throw new ArgumentNullException("cache");
            _cache = cache;
        }

        protected IObservable<T> GetDataAsync<T>(RestfullRequest<T> request, CachePolicy cachePolicy = CachePolicy.GetFromCacheAndUpdate) where T : new()
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
                                CheckIfNeededToBeUptated(request, updatable, observer);
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

        private void CheckIfNeededToBeUptated<T>(RestfullRequest<T> request, IUpdatableModel updatable,
            IObserver<T> observer)
            where T : new()
        {
            var lastUpdated = updatable.LastUpdated;
            var lastUpdatedRequest = CallFactory.GetLastUpdatedRequest<T>(request.Url);
            lastUpdatedRequest.Execute()
                .Subscribe(
                    resutl =>
                    {
                        if (resutl.Last > lastUpdated)
                        {
                            ExecuteRequest(request, observer);
                        }
                        else
                        {
                            observer.OnCompleted();
                        }
                    }, ex => observer.OnCompleted());
        }

        private void ExecuteRequest<T>(RestfullRequest<T> request, IObserver<T> observer, bool ignoreErrors = false)
            where T : new()
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
    }
}