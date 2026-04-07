using MediatR;

namespace WeatherApp.Application.Commands;

public record LoginCommand(string Email, string Password) : IRequest<string>;
