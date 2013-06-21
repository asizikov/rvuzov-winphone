using JetBrains.Annotations;
using TimeTable.Model;
using TimeTable.Networking;
using TimeTable.Networking.Restful;

namespace TimeTable.ViewModel.Restful
{
    public sealed class UniversitiesAllRequest : RestfullRequest<UniversitesAll>
    {
        public UniversitiesAllRequest(string baseUrl, string parameters, WebService webService) : base(baseUrl, webService)
        {
            AdditionalUrl = parameters;
        }

        protected override string AdditionalUrl
        {
             get; 
             set;
        }
    }

    public sealed class LastUpdatedRequest : RestfullRequest<LastUpdated>
    {
        public LastUpdatedRequest([NotNull] string baseUrl, string parameters, [NotNull] WebService webService) : base(baseUrl, webService)
        {
            AdditionalUrl = parameters;
        }

        protected override string AdditionalUrl { get; set; }
    }
}
