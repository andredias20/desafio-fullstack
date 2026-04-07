using WeatherApp.Domain.Entities;

namespace WeatherApp.Domain.Interfaces;

public interface IJwtService
{
    string GenerateToken(User user);
}
