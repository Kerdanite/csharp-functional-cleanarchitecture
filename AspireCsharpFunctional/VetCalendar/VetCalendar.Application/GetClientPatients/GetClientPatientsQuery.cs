using VetCalendar.Application.Abstractions;

namespace VetCalendar.Application.GetClientPatients;

public sealed record GetClientPatientsQuery(Guid ClientId)
    : IQuery<IReadOnlyList<PatientDto>>;