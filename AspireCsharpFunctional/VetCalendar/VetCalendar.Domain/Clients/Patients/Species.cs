using CSharpFunctionalExtensions;

namespace VetCalendar.Domain.Clients.Patients;

public sealed record Species
{
    public string Value { get; }

    private Species(string value) => Value = value;

    public static Result<Species> Create(string? species)
    {
        return Result.Success(species)
            .Ensure(
                s => !string.IsNullOrWhiteSpace(s),
                DomainErrors.Patient.SpeciesIsRequired)
            .Map(s => s!.Trim())
            .Ensure(
                s => s.Length <= 50,
                DomainErrors.Patient.SpeciesTooLong)
            .Map(s => new Species(s));
    }

    public static implicit operator string(Species species) => species.Value;
}