using MediatR;
using WeatherApp.Application.Commands;
using WeatherApp.Application.DTOs;
using WeatherApp.Domain.Entities;
using WeatherApp.Domain.Interfaces;

namespace WeatherApp.Application.Handlers;

public class RegisterTemperatureByCoordinatesHandler(IWeatherProvider weatherProvider, ITemperatureRepository repository)
    : IRequestHandler<RegisterTemperatureByCoordinatesCommand, TemperatureRecordDto>
{
    public async Task<TemperatureRecordDto> Handle(RegisterTemperatureByCoordinatesCommand request, CancellationToken ct)
    {
        var result = await weatherProvider.GetTemperatureAsync(request.Latitude, request.Longitude, ct);

        var record = new TemperatureRecord
        {
            CityName = result.CityName,
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            TemperatureCelsius = result.Temperature,
            RecordedAt = DateTime.UtcNow
        };

        await repository.AddAsync(record, ct);

        return new TemperatureRecordDto
        {
            Id = record.Id,
            CityName = record.CityName,
            Latitude = record.Latitude,
            Longitude = record.Longitude,
            TemperatureCelsius = record.TemperatureCelsius,
            RecordedAt = record.RecordedAt
        };
    }
}
