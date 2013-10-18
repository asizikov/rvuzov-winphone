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
    public abstract class BaseAsyncDataProvider
    {
        private readonly IWebCache _cache;
        protected readonly RestfulCallFactory CallFactory = new RestfulCallFactory();

        protected BaseAsyncDataProvider([NotNull] IWebCache cache)
        {
            if (cache == null) throw new ArgumentNullException("cache");
            _cache = cache;
        }

        protected IObservable<T> GetDataAsync<T>(RestfullRequest<T> request) where T : new()
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

                        var updatable = item as IUpdatableModel;
                        if (updatable != null)
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
                        else
                        {
                            observer.OnCompleted();
                        }
                    })
                );
        }

        private void ExecuteRequest<T>(RestfullRequest<T> request, IObserver<T> observer) where T : new()
        {
            request.Execute()
                .Subscribe(result =>
                {
                    _cache.Put(result, request.Url);
                    observer.OnNext(result);
                },
                    observer.OnError,
                    observer.OnCompleted);
        }
    }
}