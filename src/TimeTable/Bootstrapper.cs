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
            var stopwatch = Stopwatch.StartNew();
            Debug.WriteLine("Bootstrapper::InitApplication started");
            if (rootFrame == null) throw new ArgumentNullException("rootFrame");

            SmartDispatcher.Initialize();
            RegisterDependencies(rootFrame);
            rootFrame.UriMapper = Container.Resolve<TimeTableUriMapper>();
            Debug.WriteLine("Bootstrapper::InitApplication ended in {0} ms", stopwatch.ElapsedMilliseconds.ToString(CultureInfo.InvariantCulture));
        }


        private static void RegisterDependencies(PhoneApplicationFrame rootFrame)
        {
            Container.Initialize(rootFrame);
        }
    }
}