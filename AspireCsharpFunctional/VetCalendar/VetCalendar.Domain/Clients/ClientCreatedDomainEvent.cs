using VetCalendar.Domain.Shared;

namespace VetCalendar.Domain.Clients;

public sealed record ClientCreatedDomainEvent(ClientId ClientId) : IDomainEvent
{
    public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
}