namespace VetCalendar.Application.GetClientPatients;

public sealed record PatientDto(
    Guid Id,
    string Name,
    string Species,
    DateOnly BirthDate);