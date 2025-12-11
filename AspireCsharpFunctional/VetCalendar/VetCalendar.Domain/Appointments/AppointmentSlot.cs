using CSharpFunctionalExtensions;

namespace VetCalendar.Domain.Appointments;

public sealed record AppointmentSlot
{
    public DateOnly Date { get; }
    public TimeOnly StartTime { get; }
    public TimeSpan Duration { get; }

    private AppointmentSlot(DateOnly date, TimeOnly startTime, TimeSpan duration)
    {
        Date      = date;
        StartTime = startTime;
        Duration  = duration;
    }

    public static Result<AppointmentSlot> Create(
        DateOnly date,
        TimeOnly startTime,
        TimeSpan duration)
    {
        return Result.Success()
            .Ensure(
                () => duration > TimeSpan.Zero,
                DomainErrors.Appointment.DurationMustBePositive)
            .Ensure(
                () => duration <= TimeSpan.FromHours(4),
                DomainErrors.Appointment.DurationTooLong)
            .Map(() => new AppointmentSlot(date, startTime, duration));
    }

    public TimeOnly EndTime => StartTime.Add(Duration);
}