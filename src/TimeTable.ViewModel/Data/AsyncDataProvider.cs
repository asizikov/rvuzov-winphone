using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using TimeTable.Model;
using TimeTable.Networking.Restful;
using TimeTable.ViewModel.Restful;

namespace TimeTable.ViewModel.Data
{
    public class AsyncDataProvider
    {
        private readonly RestfulCallFactory _callFactory = new RestfulCallFactory();
        private readonly ICache _cache = new InMemoryCache();


        public IObservable<Universites> GetUniversitesAsync()
        {
            var request = _callFactory.GetAllUniversitesRequest();
            return GetDataAsync(request);
        }

        public IObservable<Groups> GetUniversitesGroupsAsync(int universityId)
        {
            var request = _callFactory.GetUniversitesGroupsRequest(universityId);
            return GetDataAsync(request);
        }

        private IObservable<T> GetDataAsync<T>(RestfullRequest<T> request) where T : new()
        {
            if (!_cache.IsCached<T>(request.Url))
            {
                return Observable.Create<T>(
                    observer =>
                        Scheduler.Default.Schedule(() => ExecuteRequest(request, observer))
                    );
            }
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
                            var lastUpdatedRequest = _callFactory.GetLastUpdatedRequest<T>();
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
                                    }
                                );
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
