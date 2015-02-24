namespace TimeTable.Mvvm.Navigation
{
    public class NavigationContext
    {
        public string From { get; set; }
        public string To { get; set; }

        public static NavigationContext Create(string from, string to)
        {
            return new NavigationContext { From = from, To = to };
        }

        public static NavigationContext<TData> Create<TData>(string from,string to, TData data)
        {
            return new NavigationContext<TData> { From = from, To = to, Body = data };
        }
    }
}