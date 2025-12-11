using CSharpFunctionalExtensions;

namespace VetCalendar.Domain.Patients;

public sealed record PetName
{
    public string Value { get; }

    private PetName(string value) => Value = value;

    public static Result<PetName> Create(string? name)
    {
        return Result.Success(name)
            .Ensure(
                n => !string.IsNullOrWhiteSpace(n),
                DomainErrors.Patient.NameIsRequired)
            .Map(n => n!.Trim())
            .Ensure(
                n => n.Length <= 100,
                DomainErrors.Patient.NameTooLong)
            .Map(n => new PetName(n));
    }

    public static implicit operator string(PetName name) => name.Value;
}