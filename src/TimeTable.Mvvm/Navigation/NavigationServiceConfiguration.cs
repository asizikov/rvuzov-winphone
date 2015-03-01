using System;
using JetBrains.Annotations;
using TimeTable.Mvvm.Navigation.Serialization;

namespace TimeTable.Mvvm.Navigation
{
    public sealed class NavigationServiceConfiguration
    {
        public NavigationServiceConfiguration()
        {
            Serializer = new NavigationSerializer();
            NavigationUriProvider = new Lazy<INavigationUriProvider>(() => new NavigationUriProvider());
        }

        [PublicAPI]
        public ISerializer Serializer { get; set; }

        [PublicAPI]
        public Lazy<INavigationUriProvider> NavigationUriProvider { get; set; }
    }
}