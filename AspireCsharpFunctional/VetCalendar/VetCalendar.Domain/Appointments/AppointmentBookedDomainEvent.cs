using VetCalendar.Domain.Shared;

namespace VetCalendar.Domain.Appointments;

public sealed record AppointmentBookedDomainEvent(AppointmentId AppointmentId) : IDomainEvent
{
    public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
}