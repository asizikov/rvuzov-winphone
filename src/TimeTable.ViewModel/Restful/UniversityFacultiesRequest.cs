using TimeTable.Model;
using TimeTable.Networking;
using TimeTable.Networking.Restful;

namespace TimeTable.ViewModel.Restful
{
    public sealed class UniversityFacultiesRequest : RestfullRequest<Faculties>
    {
        public UniversityFacultiesRequest(string parameters, WebService webService)
            : base(webService)
        {
            ResourceUrl = parameters;
        }
    }
}