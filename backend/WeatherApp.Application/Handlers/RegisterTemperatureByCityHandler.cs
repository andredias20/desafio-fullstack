using MediatR;
using WeatherApp.Application.Commands;
using WeatherApp.Application.DTOs;
using WeatherApp.Domain.Entities;
using WeatherApp.Domain.Interfaces;

namespace WeatherApp.Application.Handlers;

public class RegisterTemperatureByCityHandler(IWeatherProvider weatherProvider, ITemperatureRepository repository)
    : IRequestHandler<RegisterTemperatureByCityCommand, TemperatureRecordDto>
{
    public async Task<TemperatureRecordDto> Handle(RegisterTemperatureByCityCommand request, CancellationToken ct)
    {
        var result = await weatherProvider.GetTemperatureAsync(request.CityName, ct);

        var record = new TemperatureRecord
        {
            CityName = result.CityName,
            TemperatureCelsius = result.Temperature,
            RecordedAt = DateTime.UtcNow
        };

        await repository.AddAsync(record, ct);

        return new TemperatureRecordDto
        {
            Id = record.Id,
            CityName = record.CityName,
            TemperatureCelsius = record.TemperatureCelsius,
            RecordedAt = record.RecordedAt
        };
    }
}
