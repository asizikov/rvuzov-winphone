namespace TimeTable.ViewModel.Services
{
    public interface INotificationService
    {
        void ShowToast(string title, string message);
        void ShowSomethingWentWrongToast();
    }
}