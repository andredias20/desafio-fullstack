namespace WeatherApp.API.Requests;

public record RegisterByCityRequest
{
    public required string CityName { get; init; }
}