using System;
using System.Collections.Generic;
using TimeTable.Networking;

namespace TimeTable.ViewModel.Restful
{
    public class RestfulCallFactory
    {
        private readonly WebService webService = new WebService();

        private const string URL_PREFIX = "http://raspisanie-vuzov.ru/api/v1/";
        private const string UNIVERSITIES_ALL = "universities-all";
        private const string LAST_UPDATED = "/last-updated";
        private const string GROUPS_ALL = "universities/{id}/groups-all";

        //TODO: create calls for all API endpoints
        //universities/{id}/groups-all/last-updated list all groups for specified university last updated time
        //universities/{id}/teachers-all list all teachers for specified university
        //universities/{id}/teachers-all/last-updated list all teachers for specified university last updated time
        //groups/{id} list specified group schedule
        //groups/{id}/last-updated list specified group schedule last updated time
        //teachers/{id} list specified teacher schedule
        //teachers/{id}/last-updated list specified teacher schedule last updated time

        


        public UniversitesRequest GetAllUniversitesRequest()
        {
            return new UniversitesRequest(URL_PREFIX, UNIVERSITIES_ALL, webService);
        }

        public UniversitiesGroupsRequest GetUniversitesGroupsRequest(int universityId)
        {
            return new UniversitiesGroupsRequest(URL_PREFIX, string.Format("universities/{0}/groups-all", universityId), webService);
        }

        public LastUpdatedRequest GetLastUpdatedRequest<T>()
        {
            //todo: detect by type
            return new LastUpdatedRequest(URL_PREFIX, UNIVERSITIES_ALL + LAST_UPDATED, webService);
        }
    }
}
