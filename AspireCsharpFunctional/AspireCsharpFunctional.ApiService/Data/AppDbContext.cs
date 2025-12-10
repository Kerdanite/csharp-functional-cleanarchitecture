using Microsoft.EntityFrameworkCore;
using AspireCsharpFunctional.ApiService.Models;

namespace AspireCsharpFunctional.ApiService.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<TodoItem> Todos => Set<TodoItem>();
}