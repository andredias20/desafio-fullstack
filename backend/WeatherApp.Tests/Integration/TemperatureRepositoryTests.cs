using WeatherApp.Domain.Entities;
using WeatherApp.Infrastructure.Persistence;
using Xunit;

namespace WeatherApp.Tests.Integration;

public class TemperatureRepositoryTests : IClassFixture<PostgreSqlContainerFixture>, IAsyncLifetime
{
    private readonly PostgreSqlContainerFixture _fixture;
    private AppDbContext _context = null!;
    private TemperatureRepository _repository = null!;

    public TemperatureRepositoryTests(PostgreSqlContainerFixture fixture)
    {
        _fixture = fixture;
    }

    public async Task InitializeAsync()
    {
        _context = _fixture.CreateDbContext();
        _repository = new TemperatureRepository(_context);

        _context.TemperatureRecords.RemoveRange(_context.TemperatureRecords);
        await _context.SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        await _context.DisposeAsync();
    }

    [Fact]
    public async Task AddAsync_ShouldPersistRecord_InDatabase()
    {
        var record = new TemperatureRecord
        {
            CityName = "São Paulo",
            TemperatureCelsius = 28.0,
            RecordedAt = DateTime.UtcNow
        };

        await _repository.AddAsync(record, CancellationToken.None);

        var persisted = _context.TemperatureRecords.Single();
        Assert.Equal("São Paulo", persisted.CityName);
        Assert.Equal(28.0, persisted.TemperatureCelsius);
        Assert.True(persisted.Id > 0);
    }

    [Fact]
    public async Task GetHistoryAsync_ShouldReturnOnlyRecordsWithin30Days()
    {
        var recent = new TemperatureRecord { CityName = "São Paulo", TemperatureCelsius = 28.0, RecordedAt = DateTime.UtcNow };
        var old = new TemperatureRecord { CityName = "São Paulo", TemperatureCelsius = 33.0, RecordedAt = DateTime.UtcNow.AddDays(-31) };

        await _repository.AddAsync(recent, CancellationToken.None);
        await _repository.AddAsync(old, CancellationToken.None);

        var result = (await _repository.GetHistoryAsync("São Paulo", null, null, CancellationToken.None)).ToList();

        Assert.Single(result);
        Assert.Equal(28.0, result[0].TemperatureCelsius);
    }

    [Fact]
    public async Task GetHistoryAsync_ShouldReturnRecords_OrderedByRecordedAtDescending()
    {
        var first = new TemperatureRecord { CityName = "São Paulo", TemperatureCelsius = 26.0, RecordedAt = DateTime.UtcNow.AddHours(-2) };
        var second = new TemperatureRecord { CityName = "São Paulo", TemperatureCelsius = 28.0, RecordedAt = DateTime.UtcNow.AddHours(-1) };
        var third = new TemperatureRecord { CityName = "São Paulo", TemperatureCelsius = 30.0, RecordedAt = DateTime.UtcNow };

        await _repository.AddAsync(first, CancellationToken.None);
        await _repository.AddAsync(second, CancellationToken.None);
        await _repository.AddAsync(third, CancellationToken.None);

        var result = (await _repository.GetHistoryAsync("São Paulo", null, null, CancellationToken.None)).ToList();

        Assert.Equal(30.0, result[0].TemperatureCelsius);
        Assert.Equal(28.0, result[1].TemperatureCelsius);
        Assert.Equal(26.0, result[2].TemperatureCelsius);
    }

    [Fact]
    public async Task GetHistoryAsync_ShouldReturnEmpty_WhenNoRecordsExist()
    {
        var result = await _repository.GetHistoryAsync(null, null, null, CancellationToken.None);

        Assert.Empty(result);
    }

    [Fact]
    public async Task GetHistoryAsync_ShouldFilterByCity_AndIgnoreOtherCities()
    {
        var sp = new TemperatureRecord { CityName = "São Paulo", TemperatureCelsius = 28.0, RecordedAt = DateTime.UtcNow };
        var rj = new TemperatureRecord { CityName = "Rio de Janeiro", TemperatureCelsius = 35.0, RecordedAt = DateTime.UtcNow };

        await _repository.AddAsync(sp, CancellationToken.None);
        await _repository.AddAsync(rj, CancellationToken.None);

        var result = (await _repository.GetHistoryAsync("São Paulo", null, null, CancellationToken.None)).ToList();

        Assert.Single(result);
        Assert.Equal("São Paulo", result[0].CityName);
    }

    [Fact]
    public async Task GetHistoryAsync_ShouldFilterByCoordinates_AndIgnoreOtherLocations()
    {
        var sp = new TemperatureRecord { Latitude = -23.5, Longitude = -46.6, TemperatureCelsius = 28.0, RecordedAt = DateTime.UtcNow };
        var brasilia = new TemperatureRecord { Latitude = -15.8, Longitude = -47.9, TemperatureCelsius = 26.0, RecordedAt = DateTime.UtcNow };

        await _repository.AddAsync(sp, CancellationToken.None);
        await _repository.AddAsync(brasilia, CancellationToken.None);

        var result = (await _repository.GetHistoryAsync(null, -23.5, -46.6, CancellationToken.None)).ToList();

        Assert.Single(result);
        Assert.Equal(-23.5, result[0].Latitude);
    }

    [Fact]
    public async Task AddAsync_ShouldPersistCoordinates_WhenRegisteredByCoordinates()
    {
        var record = new TemperatureRecord
        {
            Latitude = -23.5,
            Longitude = -46.6,
            TemperatureCelsius = 28.5,
            RecordedAt = DateTime.UtcNow
        };

        await _repository.AddAsync(record, CancellationToken.None);

        var persisted = _context.TemperatureRecords.Single();
        Assert.Equal(-23.5, persisted.Latitude);
        Assert.Equal(-46.6, persisted.Longitude);
        Assert.Null(persisted.CityName);
    }
}
