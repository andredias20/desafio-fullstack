using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WeatherApp.Domain.Interfaces;
using WeatherApp.Infrastructure.Persistence;
using WeatherApp.Infrastructure.Services;
using WeatherApp.Infrastructure.WeatherProviders;

namespace WeatherApp.Infrastructure.Extensions;

public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(opt =>
            opt.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<ITemperatureRepository, TemperatureRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IJwtService, JwtService>();

        var useProvider = configuration["UseProvider"];

        if (string.Equals(useProvider, "fake", StringComparison.OrdinalIgnoreCase))
            services.AddScoped<IWeatherProvider, FakeWeatherProvider>();
        else
            services.AddHttpClient<IWeatherProvider, OpenWeatherProvider>()
                .ConfigureHttpClient(client =>
                    client.BaseAddress = new Uri(configuration["OpenWeather:BaseUrl"]!));

        return services;
    }
}
