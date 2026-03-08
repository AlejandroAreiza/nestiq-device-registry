namespace NestIQ.DeviceRegistry.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using NestIQ.DeviceRegistry.Application.UseCases;
using NestIQ.DeviceRegistry.Application.UseCases.RegisterDevice;

[ApiController]
[Route("api/devices")]
public class DevicesController : ControllerBase
{
    private readonly DeviceHandler _handler;

    public DevicesController(DeviceHandler handler)
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
            return CreatedAtAction(nameof(GetDevice), new { id = result.Id }, result);
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

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDevice(Guid id)
    {
        var result = await _handler.GetAsync(id);

        if (result is null)
            return NotFound(new { error = $"Device with id '{id}' not found." });

        return Ok(result);
    }
}