namespace TimeTable.Resources
{
    public sealed class LocalizedStrings
    {
        private static readonly Strings LocalizedResources = new Strings();

        public Strings Strings
        {
            get { return LocalizedResources; }
        }
    }
}