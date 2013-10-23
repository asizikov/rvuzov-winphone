using JetBrains.Annotations;
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

    public sealed class SendErrorRequest : RestfullRequest<Confirmation>
    {
        public SendErrorRequest([NotNull] WebService webService, [NotNull] string baseUrl,string parameters, string body) 
            : base(baseUrl, webService)
        {
            Body = body;
            Method = RequestMethod.Post;
            AdditionalUrl = parameters;
        }
    }
}