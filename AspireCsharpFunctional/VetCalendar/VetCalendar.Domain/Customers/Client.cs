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
        if (string.IsNullOrWhiteSpace(firstName))
            return Result.Failure<Client>(DomainErrors.Client.FirstNameIsRequired);

        if (string.IsNullOrWhiteSpace(lastName))
            return Result.Failure<Client>(DomainErrors.Client.LastNameIsRequired);

        if (string.IsNullOrWhiteSpace(email))
            return Result.Failure<Client>(DomainErrors.Client.EmailIsRequired);


        var client = new Client(
            ClientId.New(),
            firstName.Trim(),
            lastName.Trim(),
            email.Trim(),
            phoneNumber?.Trim() ?? string.Empty);

        return Result.Success(client);
    }
}