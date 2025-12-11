using CSharpFunctionalExtensions;

namespace VetCalendar.Domain.Clients.Patients;

public sealed record PatientId(Guid Value)
{
    public static PatientId New() => new(Guid.NewGuid());

    public override string ToString() => Value.ToString();
}