using Moq;
using WeatherApp.Application.Queries;
using WeatherApp.Domain.Entities;
using WeatherApp.Domain.Interfaces;
using Xunit;

namespace WeatherApp.Tests.Unit;

public class GetTemperatureHistoryHandlerTests
{
    private readonly Mock<ITemperatureRepository> _repository = new();
    private readonly GetTemperatureHistoryHandler _handler;

    public GetTemperatureHistoryHandlerTests()
    {
        _handler = new GetTemperatureHistoryHandler(_repository.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnMappedDtos_ForAllRecords()
    {
        var records = new List<TemperatureRecord>
        {
            new() { Id = 1, CityName = "Rio de Janeiro", TemperatureCelsius = 35.0, RecordedAt = DateTime.UtcNow },
            new() { Id = 2, Latitude = -15.8, Longitude = -47.9, TemperatureCelsius = 26.0, RecordedAt = DateTime.UtcNow.AddHours(-1) }
        };

        _repository
            .Setup(r => r.GetHistoryAsync(null, null, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(records);

        var result = (await _handler.Handle(new GetTemperatureHistoryQuery(null, null, null), CancellationToken.None)).ToList();

        Assert.Equal(2, result.Count);
        Assert.Equal("Rio de Janeiro", result[0].CityName);
        Assert.Equal(35.0, result[0].TemperatureCelsius);
        Assert.Equal(-15.8, result[1].Latitude);
        Assert.Equal(-47.9, result[1].Longitude);
        Assert.Equal(26.0, result[1].TemperatureCelsius);
    }

    [Fact]
    public async Task Handle_ShouldPassCityName_ToRepository()
    {
        _repository
            .Setup(r => r.GetHistoryAsync("São Paulo", null, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<TemperatureRecord>());

        await _handler.Handle(new GetTemperatureHistoryQuery("São Paulo", null, null), CancellationToken.None);

        _repository.Verify(r => r.GetHistoryAsync("São Paulo", null, null, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldPassCoordinates_ToRepository()
    {
        _repository
            .Setup(r => r.GetHistoryAsync(null, -23.5, -46.6, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<TemperatureRecord>());

        await _handler.Handle(new GetTemperatureHistoryQuery(null, -23.5, -46.6), CancellationToken.None);

        _repository.Verify(r => r.GetHistoryAsync(null, -23.5, -46.6, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmpty_WhenNoRecordsExist()
    {
        _repository
            .Setup(r => r.GetHistoryAsync(null, null, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<TemperatureRecord>());

        var result = await _handler.Handle(new GetTemperatureHistoryQuery(null, null, null), CancellationToken.None);

        Assert.Empty(result);
    }
}
