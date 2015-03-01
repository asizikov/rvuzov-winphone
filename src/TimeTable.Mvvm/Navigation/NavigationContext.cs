namespace TimeTable.Mvvm.Navigation
{
    public class NavigationContext
    {
        public string To { get; set; }

        public static NavigationContext Create(string to)
        {
            return new NavigationContext {To = to };
        }

        public static NavigationContext<TData> Create<TData>(string to, TData data)
        {
            return new NavigationContext<TData> {To = to, Body = data };
        }
    }
}