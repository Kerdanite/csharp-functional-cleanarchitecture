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
}