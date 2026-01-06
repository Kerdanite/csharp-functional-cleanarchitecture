
using CSharpFunctionalExtensions;

namespace VetCalendar.Domain.Clients.Patients;

public class Patient : Shared.Entity<PatientId>
{
    public PetName Name { get; private set; }
    public Species Species { get; private set; }
    public DateOnly BirthDate { get; private set; }


    private Patient(
        PatientId id,
        PetName name,
        Species species,
        DateOnly birthDate)
    {
        Id        = id;
        Name      = name;
        Species   = species;
        BirthDate = birthDate;
    }

    public static Result<Patient> Create(
        PetName name,
        Species species,
        DateOnly birthDate)
    {
            return new Patient(
                PatientId.New(),
                name,
                species,
                birthDate);
    }
}