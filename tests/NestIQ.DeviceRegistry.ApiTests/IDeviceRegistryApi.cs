namespace NestIQ.DeviceRegistry.ApiTests;
using Refit;

public interface IDeviceRegistryApi
{
    [Post("/api/devices")]
    Task<ApiResponse<RegisterDeviceResult>> RegisterDeviceAsync([Body] RegisterDeviceCommand body);

    [Get("/api/devices/{id}")]
    Task<ApiResponse<RegisterDeviceResult>> GetDeviceAsync(Guid id);
}

public record RegisterDeviceCommand(string Name, string Type, Guid HomeId);

public record RegisterDeviceResult(
    Guid Id,
    string Name,
    string Type,
    string Status,
    Guid HomeId,
    DateTime CreatedAt
);