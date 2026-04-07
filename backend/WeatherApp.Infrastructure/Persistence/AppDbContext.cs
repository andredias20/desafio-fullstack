using Microsoft.EntityFrameworkCore;
using WeatherApp.Domain.Entities;

namespace WeatherApp.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : DbContext(options)
{
    public DbSet<TemperatureRecord> TemperatureRecords { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}