using EVM.ProjectManagement.Application.Activities.DTOs;
using EVM.ProjectManagement.IntegrationTests.Fixtures;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace EVM.ProjectManagement.IntegrationTests.Activities;

public sealed class ActivitiesControllerTests : IClassFixture<ApiWebApplicationFactory>
{
    private readonly HttpClient _client;

    public ActivitiesControllerTests(ApiWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetByProjectId_ReturnsOk()
    {
        // Act
        var response = await _client.GetAsync($"/api/activities/project/{Guid.NewGuid()}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_CuandoActividadNoExiste()
    {
        // Act
        var response = await _client.GetAsync($"/api/activities/{Guid.NewGuid()}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Create_ReturnsNotFound_CuandoProyectoNoExiste()
    {
        // Arrange
        var request = new CreateActivityRequest(Guid.NewGuid(), "Test Activity", 1000, 50, 25, 300);

        // Act
        var response = await _client.PostAsJsonAsync("/api/activities", request);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
