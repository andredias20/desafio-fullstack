namespace WeatherApp.Domain.Interfaces;

public interface IWeatherProvider
{
    Task<double> GetTemperatureAsync(string cityName, CancellationToken ct);
    Task<double> GetTemperatureAsync(double latitude, double longitude, CancellationToken ct);
}