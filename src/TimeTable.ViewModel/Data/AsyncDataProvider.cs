using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using TimeTable.Model;
using TimeTable.ViewModel.Restful;

namespace TimeTable.ViewModel.Data
{
    public class AsyncDataProvider
    {
        private readonly RestfulCallFactory callFactory = new RestfulCallFactory();
        private readonly ICache cache = new InMemoryCache();


        public IObservable<UniversitesAll> GetUniversitiesAllAsync()
        {
            var request = callFactory.GetAllUniversitiesRequest();
            if (!cache.IsCached<UniversitesAll>(request.Url))
            {
                return Observable.Create<UniversitesAll>(
                    observer =>
                        Scheduler.Default.Schedule(() => ExecuteRequest(request, observer))
                    );
            }
            else
            {
                return Observable.Create<UniversitesAll>(
                    observer => 
                        Scheduler.Default.Schedule(() =>
                        {
                            var item = cache.Fetch<UniversitesAll>(request.Url);
                            var lastUpdated = item.last_updated;
                            observer.OnNext(item);
                            var lastUpdatedRequest = callFactory.GetUniversitiesLastUpdatedRequest();
                            lastUpdatedRequest.Execute()
                                .Subscribe(
                                    resutl =>
                                    {
                                        if (resutl.last_updated > lastUpdated)
                                        {
                                            ExecuteRequest(request, observer);
                                        }
                                        else
                                        {
                                            observer.OnCompleted();
                                        }
                                    }
                                );
                            
                        })
                    );
            }
        }

        private IDisposable ExecuteRequest(UniversitiesAllRequest request, IObserver<UniversitesAll> observer)
        {
            return request.Execute()
                .Subscribe(result =>
                {
                    cache.Put(result, request.Url);
                    observer.OnNext(result);
                },
                    observer.OnError,
                    observer.OnCompleted);
        }
    }
}
