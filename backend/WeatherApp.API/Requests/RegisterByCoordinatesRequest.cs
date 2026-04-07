using System.ComponentModel.DataAnnotations;

namespace WeatherApp.API.Requests;

public record RegisterByCoordinatesRequest
{
    [Range(-90, 90)]
    public double Latitude { get; init; }

    [Range(-180, 180)]
    public double Longitude { get; init; }
}
