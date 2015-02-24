namespace TimeTable.Mvvm.Navigation.Serialization
{
    public interface ISerializer
    {
        string Serialize<T>(T item);
        T Deserialize<T>(string encodedItem);
    }
}
