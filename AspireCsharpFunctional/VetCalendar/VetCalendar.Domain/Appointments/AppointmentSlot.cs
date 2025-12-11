using CSharpFunctionalExtensions;

namespace VetCalendar.Domain.Appointments;

public sealed record AppointmentSlot
{
    public DateOnly Date { get; }
    public TimeOnly StartTime { get; }

    public static readonly TimeSpan DefaultDuration = TimeSpan.FromMinutes(30);

    private AppointmentSlot(DateOnly date, TimeOnly startTime)
    {
        Date      = date;
        StartTime = startTime;
        EndTime = StartTime.Add(DefaultDuration);
    }

    public static Result<AppointmentSlot> Create(
        DateOnly date,
        TimeOnly startTime)
    {
        return Result.Success(new AppointmentSlot(date, startTime));
    }

    public TimeOnly EndTime { get; }
}