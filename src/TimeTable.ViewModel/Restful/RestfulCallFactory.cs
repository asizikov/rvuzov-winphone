using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace TimeTable.ViewModel.Restful
{
    public class RestfulCallFactory
    {
        //universities-all
        private const string URL_PREFIX = "http://raspisanie-vuzov.ru/api/v1";
        private string universities = "universities-all";

        public UniversitiesRequest GetUniversitiesRequest()
        {
            return new UniversitiesRequest(URL_PREFIX,universities);
        }
    }
}
