using MediatR;
using WeatherApp.Application.DTOs;

namespace WeatherApp.Application.Commands;

public record RegisterTemperatureByCityCommand(string CityName) : IRequest<TemperatureRecordDto>;