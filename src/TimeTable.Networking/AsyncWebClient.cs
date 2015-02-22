using JetBrains.Annotations;
using TimeTable.Networking.Cache;

namespace TimeTable.Networking
{
    public class AsyncWebClient : BaseAsyncWebClient
    {
        public AsyncWebClient([NotNull] IWebCache cache) : base(cache)
        {
        }
    }
}