namespace VetCalendar.Domain.Shared;

public interface IDomainEvent
{
    DateTime OccurredOnUtc  { get; }
}