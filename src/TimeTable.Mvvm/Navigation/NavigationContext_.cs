namespace TimeTable.Mvvm.Navigation
{
    public class NavigationContext<TData>
    {
        public string From { get; set; }
        public string To { get; set; }
        public TData Body { get; set; }
    }
}