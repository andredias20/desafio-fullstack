using Microsoft.Extensions.DependencyInjection;
using WeatherApp.Application.Queries;

namespace WeatherApp.Application.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(GetTemperatureHistoryHandler).Assembly));

        return services;
    }
}