namespace NestIQ.DeviceRegistry.ComponentTests;

using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using NestIQ.DeviceRegistry.Application.Interfaces;
using NestIQ.DeviceRegistry.Application.UseCases.RegisterDevice;
using NestIQ.DeviceRegistry.Domain.Entities;
using NestIQ.DeviceRegistry.Domain.Enums;
using Moq;
using System.Threading.Tasks;

public class DevicesControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly Mock<IDeviceRepository> _repositoryMock;

    public DevicesControllerTests(WebApplicationFactory<Program> factory)
    {
        _repositoryMock = new Mock<IDeviceRepository>();

        var customFactory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddScoped<IDeviceRepository>(_ => _repositoryMock.Object);
            });
        });

        _client = customFactory.CreateClient();
        _client.DefaultRequestHeaders.Accept.Add(
            new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
    }

    // AC7: Returns 201 with created device on success
    [Fact]
    public async Task RegisterDevice_WithValidRequest_ShouldReturn201()
    {
        // Arrange
        _repositoryMock
            .Setup(r => r.ExistsAsync(It.IsAny<Guid>(), It.IsAny<string>()))
            .ReturnsAsync(false);

        var request = new RegisterDeviceCommand(
            Name: "Living Room Light",
            Type: "Light",
            HomeId: Guid.NewGuid()
        );

        // Act
        var response = await _client.PostAsJsonAsync("/api/devices", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var options = new System.Text.Json.JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new System.Text.Json.Serialization.JsonStringEnumConverter() }
        };

        var result = await response.Content.ReadFromJsonAsync<RegisterDeviceResult>(options);
        result.Should().NotBeNull();
        result!.Name.Should().Be(request.Name);
        result.Status.Should().Be(DeviceStatus.Active);
    }

    // AC8: Returns 409 if device name already exists in same home
    [Fact]
    public async Task RegisterDevice_WhenDeviceAlreadyExists_ShouldReturn409()
    {
        // Arrange
        _repositoryMock
            .Setup(r => r.ExistsAsync(It.IsAny<Guid>(), It.IsAny<string>()))
            .ReturnsAsync(true);

        var request = new RegisterDeviceCommand(
            Name: "Living Room Light",
            Type: "Light",
            HomeId: Guid.NewGuid()
        );

        // Act
        var response = await _client.PostAsJsonAsync("/api/devices", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    // AC9: Returns 400 if validation fails
    [Fact]
    public async Task RegisterDevice_WithInvalidType_ShouldReturn400()
    {
        // Arrange
        var request = new { Name = "Light 1", Type = "InvalidType", HomeId = Guid.NewGuid() };

        // Act
        var response = await _client.PostAsJsonAsync("/api/devices", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetDevice_WhenDeviceExists_ShouldReturn200()
    {
        // Arrange
        var deviceId = Guid.NewGuid();
        var expectedResult = new RegisterDeviceResult
        {
            Id = deviceId,
            Name = "Living Room Light",
            Type = DeviceType.Light,
            Status = DeviceStatus.Active,
            HomeId = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow
        };
        _repositoryMock
        .Setup(r => r.GetByIdAsync(deviceId))
        .ReturnsAsync(Device.Create(
            expectedResult.Name,
            expectedResult.Type,
            expectedResult.HomeId));

        // Act
        var response = await _client.GetAsync($"/api/devices/{deviceId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    // 404 — device not found
    [Fact]
    public async Task GetDevice_WhenDeviceDoesNotExist_ShouldReturn404()
    {
        // Arrange
        _repositoryMock
            .Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Device?)null);

        // Act
        var response = await _client.GetAsync($"/api/devices/{Guid.NewGuid()}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}