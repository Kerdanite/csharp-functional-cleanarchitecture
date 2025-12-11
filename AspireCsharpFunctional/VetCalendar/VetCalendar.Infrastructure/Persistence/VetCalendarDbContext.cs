using Microsoft.EntityFrameworkCore;
using VetCalendar.Domain.Customers;
using VetCalendar.Domain.Patients;

namespace VetCalendar.Infrastructure.Persistence;

public class VetCalendarDbContext : DbContext
{
    public VetCalendarDbContext(DbContextOptions<VetCalendarDbContext> options)
        : base(options)
    {
    }
    public DbSet<Client> Clients => Set<Client>();
    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(VetCalendarDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}