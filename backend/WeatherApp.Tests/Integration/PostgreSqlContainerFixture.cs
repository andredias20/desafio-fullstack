using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;
using WeatherApp.Infrastructure.Persistence;
using Xunit;

namespace WeatherApp.Tests.Integration;

public class PostgreSqlContainerFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _container = new PostgreSqlBuilder()
        .WithImage("postgres:16-alpine")
        .Build();

    public AppDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql(_container.GetConnectionString())
            .Options;

        return new AppDbContext(options);
    }

    public async Task InitializeAsync()
    {
        await _container.StartAsync();

        await using var context = CreateDbContext();
        await context.Database.MigrateAsync();
    }

    public async Task DisposeAsync()
    {
        await _container.DisposeAsync();
    }
}
