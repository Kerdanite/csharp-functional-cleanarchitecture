using CSharpFunctionalExtensions;
using VetCalendar.Domain.Shared;

namespace VetCalendar.Domain.Customers;

public class Client : AggregateRoot<ClientId>
{
    public string FirstName  { get; private set; } = default!;
    public string LastName   { get; private set; } = default!;
    public string Email      { get; private set; } = default!;
    public string PhoneNumber { get; private set; } = default!;

    private Client(ClientId id, string firstName, string lastName, string email, string phoneNumber)
    {
        Id          = id;
        FirstName   = firstName;
        LastName    = lastName;
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
            .Ensure(
                () => !string.IsNullOrWhiteSpace(email),
                DomainErrors.Client.EmailIsRequired)
            .Map(() =>
                new Client(
                    ClientId.New(),
                    firstName,
                    lastName,
                    email,
                    phoneNumber));
    }
}