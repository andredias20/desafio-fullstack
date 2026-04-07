using WeatherApp.Domain.Interfaces;
using WeatherApp.Domain.Models;

namespace WeatherApp.Infrastructure.WeatherProviders;

public class FakeWeatherProvider : IWeatherProvider
{
    private static readonly string[] BrazilianCities =
    [
        "São Paulo", "Rio de Janeiro", "Brasília", "Salvador", "Fortaleza",
        "Belo Horizonte", "Manaus", "Curitiba", "Recife", "Porto Alegre",
        "Belém", "Goiânia", "Florianópolis", "Maceió", "Natal"
    ];

    public Task<WeatherResult> GetTemperatureAsync(string cityName, CancellationToken ct)
    {
        var temperature = Math.Round(Random.Shared.NextDouble() * 25 + 15, 2);
        return Task.FromResult(new WeatherResult(temperature, cityName));
    }

    public Task<WeatherResult> GetTemperatureAsync(double latitude, double longitude, CancellationToken ct)
    {
        var temperature = Math.Round(Random.Shared.NextDouble() * 25 + 15, 2);
        var city = BrazilianCities[Random.Shared.Next(BrazilianCities.Length)];
        return Task.FromResult(new WeatherResult(temperature, city));
    }
}
