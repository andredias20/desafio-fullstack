using Moq;
using WeatherApp.Application.Commands;
using WeatherApp.Application.Handlers;
using WeatherApp.Domain.Entities;
using WeatherApp.Domain.Interfaces;
using WeatherApp.Domain.Models;
using Xunit;

namespace WeatherApp.Tests.Unit;

public class RegisterTemperatureByCityHandlerTests
{
    private readonly Mock<IWeatherProvider> _weatherProvider = new();
    private readonly Mock<ITemperatureRepository> _repository = new();
    private readonly RegisterTemperatureByCityHandler _handler;

    public RegisterTemperatureByCityHandlerTests()
    {
        _handler = new RegisterTemperatureByCityHandler(_weatherProvider.Object, _repository.Object);
    }

    [Fact]
    public async Task Handle_ShouldCallWeatherProvider_WithCityName()
    {
        _weatherProvider
            .Setup(p => p.GetTemperatureAsync("São Paulo", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new WeatherResult(28.5, "São Paulo"));

        await _handler.Handle(new RegisterTemperatureByCityCommand("São Paulo"), CancellationToken.None);

        _weatherProvider.Verify(p => p.GetTemperatureAsync("São Paulo", It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldPersistRecord_WithCityNameFromProvider()
    {
        _weatherProvider
            .Setup(p => p.GetTemperatureAsync("sp", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new WeatherResult(28.5, "São Paulo"));

        var before = DateTime.UtcNow;
        await _handler.Handle(new RegisterTemperatureByCityCommand("sp"), CancellationToken.None);
        var after = DateTime.UtcNow;

        _repository.Verify(r => r.AddAsync(
            It.Is<TemperatureRecord>(rec =>
                rec.CityName == "São Paulo" &&
                rec.TemperatureCelsius == 28.5 &&
                rec.Latitude == null &&
                rec.Longitude == null &&
                rec.RecordedAt >= before &&
                rec.RecordedAt <= after),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnDto_WithCorrectData()
    {
        _weatherProvider
            .Setup(p => p.GetTemperatureAsync("São Paulo", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new WeatherResult(28.5, "São Paulo"));

        var result = await _handler.Handle(new RegisterTemperatureByCityCommand("São Paulo"), CancellationToken.None);

        Assert.Equal("São Paulo", result.CityName);
        Assert.Equal(28.5, result.TemperatureCelsius);
        Assert.Null(result.Latitude);
        Assert.Null(result.Longitude);
    }
}
