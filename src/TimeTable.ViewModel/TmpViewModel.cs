using System;
using System.Collections.ObjectModel;
using JetBrains.Annotations;
using TimeTable.Model;
using TimeTable.Networking;
using TimeTable.ViewModel.Restful;

namespace TimeTable.ViewModel
{
    public class TmpViewModel : BaseViewModel
    {
        private readonly WebService webService;
        private readonly RestfulCallFactory requestFactory;
        private ObservableCollection<University> universitiesesList;

        public TmpViewModel([NotNull] WebService webService, [NotNull] RestfulCallFactory requestFactory)
        {
            if (webService == null) throw new ArgumentNullException("webService");
            if (requestFactory == null) throw new ArgumentNullException("requestFactory");
            this.webService = webService;
            this.requestFactory = requestFactory;
            Init();
        }

        private void Init()
        {
            var universitiesRequest = requestFactory.GetAllUniversitiesRequest();

            webService.Get(universitiesRequest)
                .Subscribe(
                result =>
                {

                    UniversitiesesList = new ObservableCollection<University>(result.universities);
                },
                    ex =>
                    {
                        //handle exception
                    },
                    () =>
                    {
                        //handle loaded
                    }
                );
        }

        public ObservableCollection<University> UniversitiesesList
        {
            get { return universitiesesList; }
            set
            {
                if (Equals(value, universitiesesList)) return;
                universitiesesList = value;
                OnPropertyChanged("UniversitiesesList");
            }
        }
    }
}
