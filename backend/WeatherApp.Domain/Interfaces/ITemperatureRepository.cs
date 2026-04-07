using WeatherApp.Domain.Entities;

namespace WeatherApp.Domain.Interfaces;

public interface ITemperatureRepository
{
    Task AddAsync(TemperatureRecord record, CancellationToken ct);
    Task<IEnumerable<TemperatureRecord>> GetHistoryAsync(string? cityName, double? latitude, double? longitude, CancellationToken ct);
}