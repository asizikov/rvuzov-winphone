using System;
using System.Diagnostics;
using System.Globalization;
using JetBrains.Annotations;
using Microsoft.Phone.Controls;
using TimeTable.IoC;
using TimeTable.Mvvm;

namespace TimeTable
{
    public static class Bootstrapper
    {
        public static void InitApplication([NotNull] PhoneApplicationFrame rootFrame)
        {
            if (rootFrame == null) throw new ArgumentNullException("rootFrame");

            var stopwatch = Stopwatch.StartNew();
            Debug.WriteLine("Bootstrapper::InitApplication started");
            SmartDispatcher.Initialize(rootFrame.Dispatcher);
            RegisterDependencies(rootFrame);
            Debug.WriteLine("Bootstrapper::InitApplication resolving uri mapper at {0} ms", stopwatch.ElapsedMilliseconds.ToString(CultureInfo.InvariantCulture));
            rootFrame.UriMapper = Container.Resolve<TimeTableUriMapper>();
            Debug.WriteLine("Bootstrapper::InitApplication ended in {0} ms", stopwatch.ElapsedMilliseconds.ToString(CultureInfo.InvariantCulture));
        }


        private static void RegisterDependencies(PhoneApplicationFrame rootFrame)
        {
            Container.Initialize(rootFrame);
        }
    }
}