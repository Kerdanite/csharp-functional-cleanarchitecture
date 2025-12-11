namespace VetCalendar.Domain.Appointments;

public sealed record AppointmentId(Guid Value)
{
    public static AppointmentId New() => new(Guid.NewGuid());

    public override string ToString() => Value.ToString();
}