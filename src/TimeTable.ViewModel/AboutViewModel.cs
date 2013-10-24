using System;
using JetBrains.Annotations;
using TimeTable.ViewModel.Services;

namespace TimeTable.ViewModel
{
    public class AboutViewModel: BaseViewModel
    {
        private readonly FlurryPublisher _flurryPublisher;

        public AboutViewModel([NotNull] FlurryPublisher flurryPublisher)
        {
            if (flurryPublisher == null) throw new ArgumentNullException("flurryPublisher");
            _flurryPublisher = flurryPublisher;
            _flurryPublisher.PublishPageLoadedAbout();
        }

        [UsedImplicitly(ImplicitUseKindFlags.Access)]
        public string Version
        {
            get
            {
                return Configuration.Version;
            }
        }
    }
}
