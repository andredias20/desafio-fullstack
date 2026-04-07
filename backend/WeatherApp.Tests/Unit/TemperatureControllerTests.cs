using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WeatherApp.API.Controllers;
using WeatherApp.API.Queries;
using WeatherApp.API.Requests;
using WeatherApp.Application.Commands;
using WeatherApp.Application.DTOs;
using WeatherApp.Application.Queries;
using Xunit;

namespace WeatherApp.Tests.Unit;

public class TemperatureControllerTests
{
    private readonly Mock<IMediator> _mediator = new();
    private readonly TemperatureController _controller;

    public TemperatureControllerTests()
    {
        _controller = new TemperatureController(_mediator.Object);
    }

    [Fact]
    public async Task RegisterByCity_ShouldReturn201_WithTemperatureOnly()
    {
        var dto = new TemperatureRecordDto { Id = 1, CityName = "São Paulo", TemperatureCelsius = 28.0, RecordedAt = DateTime.UtcNow };

        _mediator
            .Setup(m => m.Send(It.IsAny<RegisterTemperatureByCityCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(dto);

        var result = await _controller.RegisterByCity(new RegisterByCityRequest { CityName = "São Paulo" });

        var created = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(201, created.StatusCode);
        var response = Assert.IsType<RegisterTemperatureResponse>(created.Value);
        Assert.Equal(28.0, response.TemperatureCelsius);
    }

    [Fact]
    public async Task RegisterByCity_ShouldSendCommand_WithCityName()
    {
        _mediator
            .Setup(m => m.Send(It.IsAny<RegisterTemperatureByCityCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new TemperatureRecordDto());

        await _controller.RegisterByCity(new RegisterByCityRequest { CityName = "Rio de Janeiro" });

        _mediator.Verify(m => m.Send(
            It.Is<RegisterTemperatureByCityCommand>(c => c.CityName == "Rio de Janeiro"),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task RegisterByCoordinates_ShouldReturn201_WithTemperatureOnly()
    {
        var dto = new TemperatureRecordDto { Id = 2, CityName = "São Paulo", Latitude = -23.5, Longitude = -46.6, TemperatureCelsius = 29.0, RecordedAt = DateTime.UtcNow };

        _mediator
            .Setup(m => m.Send(It.IsAny<RegisterTemperatureByCoordinatesCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(dto);

        var result = await _controller.RegisterByCoordinates(new RegisterByCoordinatesRequest { Latitude = -23.5, Longitude = -46.6 });

        var created = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(201, created.StatusCode);
        var response = Assert.IsType<RegisterTemperatureResponse>(created.Value);
        Assert.Equal(29.0, response.TemperatureCelsius);
    }

    [Fact]
    public async Task RegisterByCoordinates_ShouldSendCommand_WithCoordinates()
    {
        _mediator
            .Setup(m => m.Send(It.IsAny<RegisterTemperatureByCoordinatesCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new TemperatureRecordDto());

        await _controller.RegisterByCoordinates(new RegisterByCoordinatesRequest { Latitude = -23.5, Longitude = -46.6 });

        _mediator.Verify(m => m.Send(
            It.Is<RegisterTemperatureByCoordinatesCommand>(c => c.Latitude == -23.5 && c.Longitude == -46.6),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task History_ShouldReturn200_WithList()
    {
        var records = new List<TemperatureRecordDto>
        {
            new() { Id = 1, CityName = "Fortaleza", TemperatureCelsius = 32.0, RecordedAt = DateTime.UtcNow }
        };

        _mediator
            .Setup(m => m.Send(It.IsAny<GetTemperatureHistoryQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(records);

        var result = await _controller.History(new HistoryQuery { CityName = "Fortaleza" });

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(200, ok.StatusCode);
        Assert.Equal(records, ok.Value);
    }

    [Fact]
    public async Task History_ShouldSendQuery_WithCityName()
    {
        _mediator
            .Setup(m => m.Send(It.IsAny<GetTemperatureHistoryQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<TemperatureRecordDto>());

        await _controller.History(new HistoryQuery { CityName = "Salvador" });

        _mediator.Verify(m => m.Send(
            It.Is<GetTemperatureHistoryQuery>(q => q.CityName == "Salvador" && q.Latitude == null && q.Longitude == null),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task History_ShouldSendQuery_WithCoordinates()
    {
        _mediator
            .Setup(m => m.Send(It.IsAny<GetTemperatureHistoryQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<TemperatureRecordDto>());

        await _controller.History(new HistoryQuery { Latitude = -15.8, Longitude = -47.9 });

        _mediator.Verify(m => m.Send(
            It.Is<GetTemperatureHistoryQuery>(q => q.CityName == null && q.Latitude == -15.8 && q.Longitude == -47.9),
            It.IsAny<CancellationToken>()), Times.Once);
    }
}
