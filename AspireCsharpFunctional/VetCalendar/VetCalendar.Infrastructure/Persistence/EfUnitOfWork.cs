using Microsoft.EntityFrameworkCore.Storage;
using VetCalendar.Application.Abstractions;
using VetCalendar.Domain.Customers;
using VetCalendar.Domain.Shared;

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

        var domainEvents = _dbContext.ChangeTracker
            .Entries<IHasDomainEvents>() 
            .SelectMany(e => e.Entity.DomainEvents)
            .ToList();

        var outboxMessages = domainEvents
            .Select(OutboxMessage.FromDomainEvent)
            .ToList();

        await _dbContext.OutboxMessages.AddRangeAsync(outboxMessages, ct);
        
        foreach (var entry in _dbContext.ChangeTracker.Entries<IHasDomainEvents>())
        {
            entry.Entity.ClearDomainEvents();
        }

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