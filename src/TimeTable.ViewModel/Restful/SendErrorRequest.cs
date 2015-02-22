using JetBrains.Annotations;
using TimeTable.Domain;
using TimeTable.Networking;
using TimeTable.Networking.Restful;

namespace TimeTable.ViewModel.Restful
{
    public sealed class SendErrorRequest : RestfullRequest<Confirmation>
    {
        public SendErrorRequest([NotNull] WebService webService,string parameters, string body) 
            : base(webService, parameters)
        {
            Body = body;
            Method = RequestMethod.Post;
        }
    }
}