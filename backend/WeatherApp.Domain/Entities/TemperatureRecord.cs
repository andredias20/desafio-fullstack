namespace WeatherApp.Domain.Entities;

public class TemperatureRecord
{
    public long Id { get; set; }
    public string? CityName { get; set; }
    public string? Latitude { get; set; }
    public string? Longitude { get; set; }
    public double TemperatureCelsius { get; set; }
    public DateTime RecordedAt { get; set; }
}