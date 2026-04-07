using MediatR;
using WeatherApp.Application.DTOs;
using WeatherApp.Domain.Interfaces;

namespace WeatherApp.Application.Queries;

public class GetTemperatureHistoryHandler(ITemperatureRepository repository)
    : IRequestHandler<GetTemperatureHistoryQuery, IEnumerable<TemperatureRecordDto>>
{
    public async Task<IEnumerable<TemperatureRecordDto>> Handle(GetTemperatureHistoryQuery request, CancellationToken ct)
    {
        var records = await repository.GetHistoryAsync(request.CityName, request.Latitude, request.Longitude, ct);

        return records.Select(r => new TemperatureRecordDto
        {
            Id = r.Id,
            CityName = r.CityName,
            Latitude = r.Latitude,
            Longitude = r.Longitude,
            TemperatureCelsius = r.TemperatureCelsius,
            RecordedAt = r.RecordedAt
        });
    }
}