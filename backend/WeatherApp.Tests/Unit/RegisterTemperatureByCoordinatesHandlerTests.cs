using Moq;
using WeatherApp.Application.Commands;
using WeatherApp.Application.Handlers;
using WeatherApp.Domain.Entities;
using WeatherApp.Domain.Interfaces;
using WeatherApp.Domain.Models;
using Xunit;

namespace WeatherApp.Tests.Unit;

public class RegisterTemperatureByCoordinatesHandlerTests
{
    private readonly Mock<IWeatherProvider> _weatherProvider = new();
    private readonly Mock<ITemperatureRepository> _repository = new();
    private readonly RegisterTemperatureByCoordinatesHandler _handler;

    public RegisterTemperatureByCoordinatesHandlerTests()
    {
        _handler = new RegisterTemperatureByCoordinatesHandler(_weatherProvider.Object, _repository.Object);
    }

    [Fact]
    public async Task Handle_ShouldCallWeatherProvider_WithCoordinates()
    {
        _weatherProvider
            .Setup(p => p.GetTemperatureAsync(-23.5, -46.6, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new WeatherResult(31.0, "São Paulo"));

        await _handler.Handle(new RegisterTemperatureByCoordinatesCommand(-23.5, -46.6), CancellationToken.None);

        _weatherProvider.Verify(p => p.GetTemperatureAsync(-23.5, -46.6, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldPersistRecord_WithCityNameFromProvider()
    {
        _weatherProvider
            .Setup(p => p.GetTemperatureAsync(-23.5, -46.6, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new WeatherResult(31.0, "São Paulo"));

        var before = DateTime.UtcNow;
        await _handler.Handle(new RegisterTemperatureByCoordinatesCommand(-23.5, -46.6), CancellationToken.None);
        var after = DateTime.UtcNow;

        _repository.Verify(r => r.AddAsync(
            It.Is<TemperatureRecord>(rec =>
                rec.CityName == "São Paulo" &&
                rec.Latitude == -23.5 &&
                rec.Longitude == -46.6 &&
                rec.TemperatureCelsius == 31.0 &&
                rec.RecordedAt >= before &&
                rec.RecordedAt <= after),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnDto_WithCityNameFromProvider()
    {
        _weatherProvider
            .Setup(p => p.GetTemperatureAsync(-23.5, -46.6, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new WeatherResult(31.0, "São Paulo"));

        var result = await _handler.Handle(new RegisterTemperatureByCoordinatesCommand(-23.5, -46.6), CancellationToken.None);

        Assert.Equal("São Paulo", result.CityName);
        Assert.Equal(-23.5, result.Latitude);
        Assert.Equal(-46.6, result.Longitude);
        Assert.Equal(31.0, result.TemperatureCelsius);
    }
}
