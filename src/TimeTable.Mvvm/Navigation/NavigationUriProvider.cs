using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace TimeTable.Mvvm.Navigation
{
    internal class NavigationUriProvider : INavigationUriProvider
    {
        private Dictionary<Type, Type> Dictionary { get; set; }

        public NavigationUriProvider()
        {
            var assemblies = LoadAssemblies();
            var pages = assemblies.SelectMany(assembly => assembly.GetTypes())
                .Where(t => t.IsDefined(typeof(DependsOnViewModelAttribute)));
            Dictionary = pages.ToDictionary(
                p => (p.GetCustomAttribute<DependsOnViewModelAttribute>().ViewModelType),p => p);
            Debug.WriteLine("NavigationUriProvider::Initializing");
        }

        private static IEnumerable<Assembly> LoadAssemblies()
        {
            return AppDomain.CurrentDomain.GetAssemblies();
        }


        private Uri GetUri(Type viewType)
        {
            var assembly = viewType.Assembly;
            var name = assembly.GetName().Name;
            var uri = viewType.FullName.Replace(name, string.Empty).Replace(".", "/");
            return new Uri(string.Format("/{0};component{1}.xaml", name, uri), UriKind.Relative);
        }

        public Uri Get<TViewModel>() where TViewModel : BaseViewModel
        {
            if (!Dictionary.ContainsKey(typeof (TViewModel)))
            {
                throw new NavigationException("There is no mapping for " + typeof(TViewModel));
            }
            return GetUri(Dictionary[typeof(TViewModel)]);
        }
    }
}