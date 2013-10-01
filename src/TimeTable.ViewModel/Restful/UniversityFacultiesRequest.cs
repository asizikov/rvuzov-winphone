using TimeTable.Model;
using TimeTable.Networking;
using TimeTable.Networking.Restful;

namespace TimeTable.ViewModel.Restful
{
    public sealed class UniversityFacultiesRequest : RestfullRequest<Faculties>
    {
        public UniversityFacultiesRequest(string baseUrl, string parameters, WebService webService)
            : base(baseUrl, webService)
        {
            AdditionalUrl = parameters;
        }
    }
}