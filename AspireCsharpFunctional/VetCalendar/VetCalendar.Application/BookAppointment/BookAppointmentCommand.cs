using VetCalendar.Application.Abstractions;

namespace VetCalendar.Application.BookAppointment;

public sealed record BookAppointmentCommand(
    Guid ClientId,
    Guid PatientId,
    DateOnly Date,
    TimeOnly StartTime,
    string? Reason
) : ICommand;