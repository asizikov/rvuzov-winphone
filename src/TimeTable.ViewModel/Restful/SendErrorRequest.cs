using JetBrains.Annotations;
using TimeTable.Model;
using TimeTable.Networking;
using TimeTable.Networking.Restful;

namespace TimeTable.ViewModel.Restful
{
    public sealed class SendErrorRequest : RestfullRequest<Confirmation>
    {
        public SendErrorRequest([NotNull] WebService webService,string parameters, string body) 
            : base(webService)
        {
            Body = body;
            Method = RequestMethod.Post;
            ResourceUrl = parameters;
        }
    }
}