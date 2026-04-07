using Microsoft.AspNetCore.Mvc;

namespace WeatherApp.API.Queries;

public record HistoryQuery
{
    [FromQuery(Name = "city")]
    public string? CityName { get; init; }
    public double? Latitude { get; init; }
    public double? Longitude { get; init; }
}