using TimeTable.Model;
using TimeTable.Networking.Restful;

namespace TimeTable.ViewModel.Restful
{
    public sealed class UniversitiesRequest : RestfullRequest<Universities>
    {
        public UniversitiesRequest(string baseUrl, string parameters) : base(baseUrl)
        {
            AdditionalUrl = parameters;
        }

        protected override string AdditionalUrl
        {
             get; 
             set;
        }
    }
}
