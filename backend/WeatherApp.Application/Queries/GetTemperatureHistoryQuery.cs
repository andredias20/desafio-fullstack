using MediatR;
using WeatherApp.Application.DTOs;

namespace WeatherApp.Application.Queries;

public record GetTemperatureHistoryQuery(string? CityName, double? Latitude, double? Longitude) : IRequest<IEnumerable<TemperatureRecordDto>>;