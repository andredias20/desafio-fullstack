namespace WeatherApp.Application.DTOs;

public class TemperatureRecordDto
{
    public long Id { get; set; }
    public string? CityName { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public double TemperatureCelsius { get; set; }
    public DateTime RecordedAt { get; set; }
}