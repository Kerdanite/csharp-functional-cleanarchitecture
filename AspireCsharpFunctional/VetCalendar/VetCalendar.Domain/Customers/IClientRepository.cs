namespace VetCalendar.Domain.Customers;

public interface IClientRepository
{
    Task AddAsync(Client client, CancellationToken ct = default);

    Task<bool> EmailExistsAsync(string email, CancellationToken ct = default);
    Task<bool> PhoneNumberExistsAsync(string phoneNumber, CancellationToken ct = default);
}