using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WeatherApp.API.Queries;
using WeatherApp.API.Requests;
using WeatherApp.Application.Commands;
using WeatherApp.Application.DTOs;
using WeatherApp.Application.Queries;

namespace WeatherApp.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TemperatureController(IMediator mediator) : ControllerBase
{
    [HttpGet("history")]
    public async Task<ActionResult<IEnumerable<TemperatureRecordDto>>> History([FromQuery] HistoryQuery query)
    {
        var result = await mediator.Send(new GetTemperatureHistoryQuery(
            query.CityName,
            query.Latitude,
            query.Longitude));

        return Ok(result);
    }

    [HttpPost("coordinates")]
    public async Task<ActionResult<RegisterTemperatureResponse>> RegisterByCoordinates([FromBody] RegisterByCoordinatesRequest request)
    {
        var result = await mediator.Send(new RegisterTemperatureByCoordinatesCommand(
            request.Latitude,
            request.Longitude));

        return CreatedAtAction(nameof(History), new RegisterTemperatureResponse(result.TemperatureCelsius));
    }

    [HttpPost("city")]
    public async Task<ActionResult<RegisterTemperatureResponse>> RegisterByCity([FromBody] RegisterByCityRequest request)
    {
        var result = await mediator.Send(new RegisterTemperatureByCityCommand(
            request.CityName));

        return CreatedAtAction(nameof(History), new RegisterTemperatureResponse(result.TemperatureCelsius));
    }
}
