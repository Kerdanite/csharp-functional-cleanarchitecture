using CSharpFunctionalExtensions;
using VetCalendar.Domain.Clients;
using VetCalendar.Domain.Clients.Patients;
using VetCalendar.Domain.Shared;

namespace VetCalendar.Domain.Appointments;

public class Appointment : AggregateRoot<AppointmentId>
{
    public ClientId ClientId { get; private set; }
    public PatientId PatientId { get; private set; }
    public AppointmentSlot Slot { get; private set; }
    public string Reason { get; private set; }
    public AppointmentStatus Status { get; private set; }

    private Appointment(){} // EF fait chier faut je trouver un workaround pour s'en passer

    private Appointment(
        AppointmentId id,
        ClientId clientId,
        PatientId patientId,
        AppointmentSlot slot,
        string reason,
        AppointmentStatus status)
    {
        Id        = id;
        ClientId  = clientId;
        PatientId = patientId;
        Slot      = slot;
        Reason    = reason;
        Status    = status;
    }

    public static Result<Appointment> Book(
        ClientId clientId,
        PatientId patientId,
        AppointmentSlot slot,
        string? reason)
    {
        return Result.Success(reason)
            .Map(m =>  m?.Trim() ?? string.Empty)
            .Ensure(
                r => r.Length <= 500,
                DomainErrors.Appointment.ReasonTooLong)
            .Map(r => new Appointment(
                id: AppointmentId.New(),
                clientId: clientId,
                patientId: patientId,
                slot: slot,
                reason: r,
                status: AppointmentStatus.Booked))
            .Tap(a => a.AddDomainEvent(new AppointmentBookedDomainEvent(a.Id)));
    }
}