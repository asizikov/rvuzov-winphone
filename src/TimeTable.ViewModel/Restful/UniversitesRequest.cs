using TimeTable.Model;
using TimeTable.Networking;
using TimeTable.Networking.Restful;

namespace TimeTable.ViewModel.Restful
{
    public sealed class UniversitesRequest : RestfullRequest<Universities>
    {
        public UniversitesRequest(string parameters, WebService webService) : base(webService)
        {
            ResourceUrl = parameters;
        }
    }
}
