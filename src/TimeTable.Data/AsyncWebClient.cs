using JetBrains.Annotations;
using TimeTable.Networking.Cache;

namespace TimeTable.Data
{
    public class AsyncWebClient : BaseAsyncWebClient
    {
        public AsyncWebClient([NotNull] IWebCache cache) : base(cache)
        {
        }
    }
}