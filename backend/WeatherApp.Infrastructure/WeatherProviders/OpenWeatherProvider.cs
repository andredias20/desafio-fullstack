using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;
using WeatherApp.Domain.Interfaces;
using WeatherApp.Domain.Models;

namespace WeatherApp.Infrastructure.WeatherProviders;

public class OpenWeatherProvider : IWeatherProvider
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public OpenWeatherProvider(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _apiKey = configuration["OpenWeather:ApiKey"]!;
    }

    public async Task<WeatherResult> GetTemperatureAsync(string cityName, CancellationToken ct)
    {
        var response = await _httpClient.GetAsync($"weather?q={cityName}&appid={_apiKey}&units=metric&lang=pt_br", ct);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadFromJsonAsync<OpenWeatherResponse>(ct);
        return new WeatherResult(content!.Main.Temp, content.Name);
    }

    public async Task<WeatherResult> GetTemperatureAsync(double latitude, double longitude, CancellationToken ct)
    {
        var response = await _httpClient.GetAsync($"weather?lat={latitude}&lon={longitude}&appid={_apiKey}&units=metric&lang=pt_br", ct);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadFromJsonAsync<OpenWeatherResponse>(ct);
        return new WeatherResult(content!.Main.Temp, content.Name);
    }

    private record OpenWeatherResponse(
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("main")] Main Main);

    private record Main([property: JsonPropertyName("temp")] double Temp);
}
