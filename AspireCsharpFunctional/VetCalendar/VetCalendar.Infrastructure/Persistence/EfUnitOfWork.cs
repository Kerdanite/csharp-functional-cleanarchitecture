using Microsoft.EntityFrameworkCore.Storage;
using VetCalendar.Application.Abstractions;

namespace VetCalendar.Infrastructure.Persistence;

public class EfUnitOfWork : IUnitOfWork
{
    private readonly VetCalendarDbContext _dbContext;
    private IDbContextTransaction? _currentTransaction;

    public EfUnitOfWork(VetCalendarDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task BeginAsync(CancellationToken ct = default)
    {
        if (_currentTransaction != null)
            return; // déjà dans une transaction

        _currentTransaction = await _dbContext.Database.BeginTransactionAsync(ct);
    }

    public async Task CommitAsync(CancellationToken ct = default)
    {
        if (_currentTransaction == null)
            return;

        await _dbContext.SaveChangesAsync(ct);
        await _currentTransaction.CommitAsync(ct);
        await _currentTransaction.DisposeAsync();
        _currentTransaction = null;
    }

    public async Task RollbackAsync(CancellationToken ct = default)
    {
        if (_currentTransaction == null)
            return;

        await _currentTransaction.RollbackAsync(ct);
        await _currentTransaction.DisposeAsync();
        _currentTransaction = null;
    }
}