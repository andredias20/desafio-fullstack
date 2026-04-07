using MediatR;

namespace WeatherApp.Application.Commands;

public record RegisterUserCommand(string Email, string Password) : IRequest<Unit>;
