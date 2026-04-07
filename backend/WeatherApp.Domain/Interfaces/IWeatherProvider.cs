using WeatherApp.Domain.Models;

namespace WeatherApp.Domain.Interfaces;

public interface IWeatherProvider
{
    Task<WeatherResult> GetTemperatureAsync(string cityName, CancellationToken ct);
    Task<WeatherResult> GetTemperatureAsync(double latitude, double longitude, CancellationToken ct);
}
