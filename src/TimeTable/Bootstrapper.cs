using System;
using System.Windows.Threading;
using JetBrains.Annotations;
using Microsoft.Phone.Controls;
using TimeTable.IoC;
using TimeTable.Services;
using TimeTable.ViewModel.Data;
using TimeTable.ViewModel.Services;

namespace TimeTable
{
    public static class Bootstrapper
    {
        public static void InitApplication([NotNull] PhoneApplicationFrame rootFrame)
        {
            if (rootFrame == null) throw new ArgumentNullException("rootFrame");

            SmartDispatcher.Initialize();
            RegisterDependencies(rootFrame);
        }


        private static void RegisterDependencies(PhoneApplicationFrame rootFrame)
        {
            var ioc = ContainerInstance.Current;
            ioc.Register<INavigationService>(new NavigationService(rootFrame));
            ioc.Register<BaseApplicationSettings>(new ApplicationSettings());
            ioc.Register<FlurryPublisher>(new DebugFlurryPublisher());
            ioc.Register<ICache>(new InMemoryCache());
        }
    }
}
