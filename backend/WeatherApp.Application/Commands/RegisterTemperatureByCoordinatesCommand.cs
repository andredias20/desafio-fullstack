using MediatR;
using WeatherApp.Application.DTOs;

namespace WeatherApp.Application.Commands;

public record RegisterTemperatureByCoordinatesCommand(double Latitude, double Longitude)
    : IRequest<TemperatureRecordDto>;