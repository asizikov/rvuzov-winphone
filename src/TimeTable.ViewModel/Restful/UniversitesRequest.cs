using JetBrains.Annotations;
using TimeTable.Model;
using TimeTable.Networking;
using TimeTable.Networking.Restful;

namespace TimeTable.ViewModel.Restful
{
    public sealed class UniversitesRequest : RestfullRequest<Universites>
    {
        public UniversitesRequest(string baseUrl, string parameters, WebService webService) : base(baseUrl, webService)
        {
            AdditionalUrl = parameters;
        }

        protected override string AdditionalUrl
        {
             get; 
             set;
        }
    }

    public sealed class UniversitiesGroupsRequest : RestfullRequest<Groups>
    {
        public UniversitiesGroupsRequest(string baseUrl, string parameters, WebService webService): base(baseUrl, webService)
        {
            AdditionalUrl = parameters;
        }

        protected override string AdditionalUrl
        {
             get; 
             set;
        }
    }

    public sealed class LastUpdatedRequest : RestfullRequest<Updates>
    {
        public LastUpdatedRequest([NotNull] string baseUrl, string parameters, [NotNull] WebService webService) : base(baseUrl, webService)
        {
            AdditionalUrl = parameters;
        }

        protected override string AdditionalUrl { get; set; }
    }
}
