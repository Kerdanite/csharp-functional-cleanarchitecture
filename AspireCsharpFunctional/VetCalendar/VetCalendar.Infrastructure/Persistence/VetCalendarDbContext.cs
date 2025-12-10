using Microsoft.EntityFrameworkCore;
using VetCalendar.Domain.Customers;

namespace VetCalendar.Infrastructure.Persistence;

public class VetCalendarDbContext : DbContext
{
    public VetCalendarDbContext(DbContextOptions<VetCalendarDbContext> options)
        : base(options)
    {
    }
    public DbSet<Client> Clients => Set<Client>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(VetCalendarDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}