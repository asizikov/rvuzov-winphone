namespace TimeTable.ViewModel.Restful
{
    public class RestfulCallFactory
    {
        //universities-all
        //universities-all list all universities
        //universities-all/last-updated list all universities last updated time
        //universities/{id}/groups-all list all groups for specified university
        //universities/{id}/groups-all/last-updated list all groups for specified university last updated time
        //universities/{id}/teachers-all list all teachers for specified university
        //universities/{id}/teachers-all/last-updated list all teachers for specified university last updated time
        //groups/{id} list specified group schedule
        //groups/{id}/last-updated list specified group schedule last updated time
        //teachers/{id} list specified teacher schedule
        //teachers/{id}/last-updated list specified teacher schedule last updated time

        
        private const string URL_PREFIX = "http://raspisanie-vuzov.ru/api/v1/";
        private const string UNIVERSITIES = "universities-all";

        public UniversitiesAllRequest GetAllUniversitiesRequest()
        {
            return new UniversitiesAllRequest(URL_PREFIX, UNIVERSITIES);
        }
    }
}
