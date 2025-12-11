using Microsoft.EntityFrameworkCore;
using VetCalendar.Domain.Appointments;

namespace VetCalendar.Infrastructure.Persistence;

public class AppointmentRepository : IAppointmentRepository
{
    private readonly VetCalendarDbContext _db;

    public AppointmentRepository(VetCalendarDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(Appointment appointment, CancellationToken ct = default)
    {
        await _db.Appointments.AddAsync(appointment, ct);
    }

    public async Task<bool> IsSlotAvailableAsync(
        DateOnly date,
        TimeOnly startTime,
        CancellationToken ct = default)
    {
        return !await _db.Appointments
            .AnyAsync(a =>
                    a.Slot.Date == date &&
                    a.Slot.StartTime == startTime,
                ct);
    }
}