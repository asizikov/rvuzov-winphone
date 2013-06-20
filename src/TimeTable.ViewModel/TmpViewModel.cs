using System;
using JetBrains.Annotations;
using TimeTable.Model;
using TimeTable.Networking;
using TimeTable.ViewModel.Restful;

namespace TimeTable.ViewModel
{
    public class TmpViewModel : BaseViewModel
    {
        private readonly WebService webService;
        private readonly RestfulCallFactory factory;
        private Universities universities;

        public TmpViewModel([NotNull] WebService webService, [NotNull] RestfulCallFactory factory)
        {
            if (webService == null) throw new ArgumentNullException("webService");
            if (factory == null) throw new ArgumentNullException("factory");
            this.webService = webService;
            this.factory = factory;
            Init();
        }

        private void Init()
        {
            webService.Get(factory.GetUniversitiesRequest())
                .Subscribe(
                result =>
                {

                    Universities = result;
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

        [CanBeNull]
        public Universities Universities
        {
            get { return universities; }
            set
            {
                if (Equals(value, universities)) return;
                universities = value;
                OnPropertyChanged("Universities");
            }
        }
    }
}
