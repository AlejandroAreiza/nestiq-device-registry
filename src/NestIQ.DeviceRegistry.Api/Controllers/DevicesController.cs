namespace NestIQ.DeviceRegistry.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using NestIQ.DeviceRegistry.Application.UseCases.RegisterDevice;
using NestIQ.DeviceRegistry.Domain.Enums;

[ApiController]
[Route("api/devices")]
public class DevicesController : ControllerBase
{
    private readonly RegisterDeviceHandler _handler;

    public DevicesController(RegisterDeviceHandler handler)
    {
        _handler = handler;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> RegisterDevice([FromBody] RegisterDeviceCommand command)
    {
        try
        {
            var result = await _handler.RegisterAsync(command);
            return CreatedAtAction(nameof(RegisterDevice), new { id = result.Id }, result);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { error = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}