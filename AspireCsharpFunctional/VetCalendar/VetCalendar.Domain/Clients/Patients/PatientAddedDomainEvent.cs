using VetCalendar.Domain.Shared;

namespace VetCalendar.Domain.Clients.Patients;

public sealed record PatientAddedDomainEvent(
    ClientId ClientId,
    PatientId PatientId
) : IDomainEvent
{
    public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
}