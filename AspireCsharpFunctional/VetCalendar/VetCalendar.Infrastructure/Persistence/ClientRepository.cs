using Microsoft.EntityFrameworkCore;
using VetCalendar.Domain.Customers;

namespace VetCalendar.Infrastructure.Persistence;

public class ClientRepository : IClientRepository
{
    private readonly VetCalendarDbContext _db;

    public ClientRepository(VetCalendarDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(Client client, CancellationToken ct = default)
    {
        await _db.Clients.AddAsync(client, ct);
    }

    public async Task<bool> EmailExistsAsync(string email, CancellationToken ct = default)
    {
        return await _db.Clients
            .AnyAsync(c => c.Email == email, ct);
    }

    public async Task<bool> PhoneNumberExistsAsync(string phoneNumber, CancellationToken ct = default)
    {
        return await _db.Clients
            .AnyAsync(c => c.PhoneNumber == phoneNumber, ct);
    }

    public async Task<Client?> GetByIdAsync(ClientId id, CancellationToken ct = default)
    {
        return await _db.Clients
            .FirstOrDefaultAsync(c => c.Id == id, ct);
    }
}