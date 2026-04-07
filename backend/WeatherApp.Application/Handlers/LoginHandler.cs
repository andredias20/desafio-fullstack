using MediatR;
using WeatherApp.Application.Commands;
using WeatherApp.Domain.Interfaces;

namespace WeatherApp.Application.Handlers;

public class LoginHandler(IUserRepository userRepository, IJwtService jwtService) : IRequestHandler<LoginCommand, string>
{
    public async Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByEmailAsync(request.Email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Credenciais inválidas.");

        return jwtService.GenerateToken(user);
    }
}
