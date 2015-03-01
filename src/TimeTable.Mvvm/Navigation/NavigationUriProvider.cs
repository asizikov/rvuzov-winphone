using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Controls;

namespace TimeTable.Mvvm.Navigation
{
    internal class NavigationUriProvider : INavigationUriProvider
    {
        private Dictionary<Type, Type> Dictionary { get; set; }

        public NavigationUriProvider()
        {
            ThreadPool.QueueUserWorkItem(o => Initialize());
        }

        private void Initialize()
        {
            var stopwatch = Stopwatch.StartNew();
            Debug.WriteLine("NavigationUriProvider::Initializing");
            var assemblies = LoadAssemblies();
            Debug.WriteLine("NavigationUriProvider:: assemblies loaded at {0} ms",
                stopwatch.ElapsedMilliseconds.ToString(CultureInfo.InvariantCulture));
            var pages =
                assemblies.SelectMany(
                    assembly =>
                        assembly.GetTypes()
                            .Where(
                                t =>
                                    t.IsPublic && t.IsClass && t.IsSubclassOf(typeof (Page)) &&
                                    t.IsDefined(typeof (DependsOnViewModelAttribute))))
                    .ToList();
            Debug.WriteLine("NavigationUriProvider:: pages selected loaded at {0} ms",
                stopwatch.ElapsedMilliseconds.ToString(CultureInfo.InvariantCulture));
            Dictionary = pages.ToDictionary(
                p => (p.GetCustomAttribute<DependsOnViewModelAttribute>().ViewModelType), p => p);
            Debug.WriteLine("NavigationUriProvider::Initialed in {0} ms",
                stopwatch.ElapsedMilliseconds.ToString(CultureInfo.InvariantCulture));
        }

        private static IEnumerable<Assembly> LoadAssemblies()
        {
            return AppDomain.CurrentDomain.GetAssemblies().Where(a => a.GetCustomAttribute<ContainsNavigationDestinations>() != null);
        }


        public Uri Get<TViewModel>() where TViewModel : BaseViewModel
        {
            if (!Dictionary.ContainsKey(typeof (TViewModel)))
            {
                throw new NavigationException("There is no mapping for " + typeof (TViewModel));
            }
            return Dictionary[typeof (TViewModel)].GetUri();
        }
    }
}