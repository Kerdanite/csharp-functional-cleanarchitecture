using CSharpFunctionalExtensions;
using VetCalendar.Application.Abstractions;

namespace VetCalendar.Application.CreateClient;

public sealed record CreateClientCommand(string FirstName, string LastName, string Email, string PhoneNumber): ICommand;