namespace TimeTable.Mvvm
{
    public abstract class PageViewModel<TData> : BaseViewModel
    {
        public abstract void Initialize(TData navigationParameter);
    }

    public abstract class PageViewModel : BaseViewModel
    {
    }
}