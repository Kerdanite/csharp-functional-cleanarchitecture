using VetCalendar.Application.Abstractions;

namespace VetCalendar.Application.AddPatient;

public sealed record AddPatientCommand(Guid ClientId, string Name, string Species, DateOnly BirthDate): ICommand;
