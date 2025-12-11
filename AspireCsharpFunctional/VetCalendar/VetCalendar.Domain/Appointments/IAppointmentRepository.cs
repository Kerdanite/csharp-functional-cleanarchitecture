namespace VetCalendar.Domain.Appointments;

public interface IAppointmentRepository
{
    Task AddAsync(Appointment appointment, CancellationToken ct = default);

    Task<bool> IsSlotAvailableAsync(DateOnly date, TimeOnly startTime, CancellationToken ct = default);
}