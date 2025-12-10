using CSharpFunctionalExtensions;
using System.Text.RegularExpressions;

namespace VetCalendar.Domain.Customers;

public sealed record Email
{
    public string Value { get; }

    private Email(string value)
    {
        Value = value;
    }

    public static Result<Email> Create(string? email)
    {

        return Result.Success(email)
            .Ensure(value => !string.IsNullOrWhiteSpace(value), DomainErrors.Client.EmailIsRequired)
            .Map(value => value.Trim())
            .Ensure(value => Regex.IsMatch(value, @"^\S+@\S+\.\S+$"), DomainErrors.Client.EmailIsInvalid)
            .Map(value => new Email(value));
    }

    public static implicit operator string(Email email) =>
        email.Value;
}