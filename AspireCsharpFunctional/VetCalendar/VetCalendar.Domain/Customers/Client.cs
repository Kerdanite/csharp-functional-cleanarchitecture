using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.ValueTasks;
using VetCalendar.Domain.Patients;
using VetCalendar.Domain.Shared;

namespace VetCalendar.Domain.Customers;

public class Client : AggregateRoot<ClientId>
{
    public string FirstName  { get; private set; }
    public string LastName   { get; private set; } 
    public PhoneNumber PhoneNumber { get; private set; }
    public Email Email { get; private set; }

    private readonly List<Patient> _patients = new();
    public IReadOnlyCollection<Patient> Patients => _patients.AsReadOnly();

    private Client(ClientId id, string firstName, string lastName, Email email, PhoneNumber phoneNumber)
    {
        Id          = id;
        FirstName   = firstName.Trim();
        LastName    = lastName.Trim();
        Email       = email;
        PhoneNumber = phoneNumber;
    }

    public static Result<Client> Create(string firstName, string lastName, string email, string phoneNumber)
    {
        return Result.Success()
            .Ensure(
                () => !string.IsNullOrWhiteSpace(firstName),
                DomainErrors.Client.FirstNameIsRequired)
            .Ensure(
                () => !string.IsNullOrWhiteSpace(lastName),
                DomainErrors.Client.LastNameIsRequired)
            .Bind(() => Email.Create(email))
            .Bind(emailVo => PhoneNumber.Create(phoneNumber).Map(phoneVo => (Email: emailVo, Phone: phoneVo)))
            .Map(tuple => new Client(
                ClientId.New(),
                firstName.Trim(),
                lastName.Trim(),
                tuple.Email,
                tuple.Phone))
            .Tap(client => client.AddDomainEvent(new ClientCreatedDomainEvent(client.Id)));
    }

    public Result<Patient> AddPatient(
        string name,
        string species,
        DateOnly birthDate)
    {
        var nameResult    = PetName.Create(name);
        var speciesResult = Species.Create(species);

        return Result
            .Combine(nameResult, speciesResult)
            .Bind(() =>
                Patient.Create(
                    nameResult.Value,
                    speciesResult.Value,
                    birthDate))
            .Tap(patient => _patients.Add(patient))
            .Tap(patient => AddDomainEvent(new PatientAddedDomainEvent(Id, patient.Id)));
    }
}