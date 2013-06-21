﻿using TimeTable.Networking;

namespace TimeTable.ViewModel.Restful
{
    public class RestfulCallFactory
    {
        private readonly WebService webService = new WebService();

        private const string URL_PREFIX = "http://raspisanie-vuzov.ru/api/v1/";
        private const string UNIVERSITIES_ALL = "universities-all";
        private const string UIVERSITIES_ALL_LAST_UPDATED = "universities-all/last-updated";
        private const string GROUPS_ALL = "universities/{id}/groups-all";

        //TODO: create calls for all API endpoints
        //universities/{id}/groups-all/last-updated list all groups for specified university last updated time
        //universities/{id}/teachers-all list all teachers for specified university
        //universities/{id}/teachers-all/last-updated list all teachers for specified university last updated time
        //groups/{id} list specified group schedule
        //groups/{id}/last-updated list specified group schedule last updated time
        //teachers/{id} list specified teacher schedule
        //teachers/{id}/last-updated list specified teacher schedule last updated time

        


        public UniversitiesAllRequest GetAllUniversitiesRequest()
        {
            return new UniversitiesAllRequest(URL_PREFIX, UNIVERSITIES_ALL, webService);
        }

        public LastUpdatedRequest GetUniversitiesLastUpdatedRequest()
        {
            return new LastUpdatedRequest(URL_PREFIX, UIVERSITIES_ALL_LAST_UPDATED, webService);
        }
    }
}