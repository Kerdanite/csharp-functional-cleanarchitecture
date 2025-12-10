using CSharpFunctionalExtensions;
using System.Text.RegularExpressions;

namespace VetCalendar.Domain.Customers;

public sealed record PhoneNumber
{
    public string Value { get; }

    private PhoneNumber(string value)
    {
        Value = value;
    }

    public static Result<PhoneNumber> Create(string? phoneNumber)
    {
        return Result.Success(phoneNumber)
            .Ensure(value => !string.IsNullOrWhiteSpace(value), DomainErrors.Client.PhoneNumberIsRequired)
            .Map(value => value.Trim())
            .Ensure(value => Regex.IsMatch(value, @"^\+\d{6,}$"), DomainErrors.Client.PhoneNumberIsInvalid)
            .Map(value => new PhoneNumber(value));
    }

    public static implicit operator string(PhoneNumber phone) => phone.Value;
}