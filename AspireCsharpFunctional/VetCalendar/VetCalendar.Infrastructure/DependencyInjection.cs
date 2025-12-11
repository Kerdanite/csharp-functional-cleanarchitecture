using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VetCalendar.Application.Abstractions;
using VetCalendar.Domain.Clients;
using VetCalendar.Infrastructure.Persistence;

namespace VetCalendar.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("apidb");
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException("Connection string 'apidb' is not configured.");

        services.AddDbContext<VetCalendarDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });

        services.AddScoped<IUnitOfWork, EfUnitOfWork>();

        services.AddScoped<IClientRepository, ClientRepository>();

        return services;
    }
}