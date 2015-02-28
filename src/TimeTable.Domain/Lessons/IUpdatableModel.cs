namespace TimeTable.Domain.Lessons
{
    public interface IUpdatableModel
    {
        long LastUpdated { get; }
    }
}