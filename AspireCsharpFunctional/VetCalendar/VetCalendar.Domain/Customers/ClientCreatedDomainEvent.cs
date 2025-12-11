using VetCalendar.Domain.Shared;

namespace VetCalendar.Domain.Customers;

public sealed record ClientCreatedDomainEvent(ClientId ClientId) : IDomainEvent
{
    public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
}