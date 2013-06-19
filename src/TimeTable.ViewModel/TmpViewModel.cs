using System;
using JetBrains.Annotations;
using TimeTable.Model;
using TimeTable.Networking;

namespace TimeTable.ViewModel
{
    public class TmpViewModel : BaseViewModel
    {
        private readonly WebService webService;
        private Universities universities;

        public TmpViewModel([NotNull] WebService webService)
        {
            if (webService == null) throw new ArgumentNullException("webService");
            this.webService = webService;
            Init();
        }

        private void Init()
        {
            webService.Get<Universities>("universities-all")
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
