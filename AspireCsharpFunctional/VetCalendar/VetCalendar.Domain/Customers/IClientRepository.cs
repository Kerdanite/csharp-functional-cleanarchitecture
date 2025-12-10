namespace VetCalendar.Domain.Customers;

public interface IClientRepository
{
    Task AddAsync(Client client, CancellationToken ct = default);
}