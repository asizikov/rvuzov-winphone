using JetBrains.Annotations;
using TimeTable.Mvvm.Navigation.Serialization;

namespace TimeTable.Mvvm.Navigation
{
    public sealed class NavigationServiceConfiguration
    {
        public NavigationServiceConfiguration()
        {
            Serializer = new NavigationSerializer();
            NavigationUriProvider = new NavigationUriProvider();
        }

        [PublicAPI]
        public ISerializer Serializer { get; set; }

        [PublicAPI]
        public INavigationUriProvider NavigationUriProvider { get; set; }
    }
}