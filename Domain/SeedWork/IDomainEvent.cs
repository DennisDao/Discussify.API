namespace Domain.SeedWork
{
    public interface IDomainEvent
    {
        string EventType { get; }
    }
}
