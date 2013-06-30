using Funq;

namespace TimeTable.IoC
{
    public sealed class ContainerInstance
    {
        private ContainerInstance()
        {
        }

        public static Container Current { get { return Nested.Instance; } }

        private static class Nested
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Nested()
            {
            }

            internal static readonly Container Instance = new Container();
        }
    }
}
