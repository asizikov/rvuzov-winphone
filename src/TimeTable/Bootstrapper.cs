using System.Windows.Threading;

namespace TimeTable
{
    public class Bootstrapper
    {
        public static void InitApplication()
        {
            SmartDispatcher.Initialize();
        }
    }
}
