namespace VetCalendar.Application.GetClients;

public record ClientDto(Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber);