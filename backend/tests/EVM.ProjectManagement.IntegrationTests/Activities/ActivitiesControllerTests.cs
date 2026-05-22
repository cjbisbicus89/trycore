namespace EVM.ProjectManagement.IntegrationTests.Activities;

using System.Net;
using System.Net.Http.Json;
using EVM.ProjectManagement.Application.Activities.DTOs;
using EVM.ProjectManagement.Application.Projects.DTOs;
using EVM.ProjectManagement.IntegrationTests.Fixtures;
using Xunit;

public sealed class ActivitiesControllerTests : IClassFixture<ApiWebApplicationFactory>
{
    private readonly HttpClient client;

    public ActivitiesControllerTests(ApiWebApplicationFactory factory)
    {
        this.client = factory.CreateClient();
    }

    [Fact]
    public async Task GetByProjectIdReturnsOk()
    {
        // Act
        var response = await this.client.GetAsync($"/api/activities/project/{Guid.NewGuid()}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetByIdReturnsNotFoundCuandoActividadNoExiste()
    {
        // Act
        var response = await this.client.GetAsync($"/api/activities/{Guid.NewGuid()}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task CreateReturnsNotFoundCuandoProyectoNoExiste()
    {
        // Arrange
        var request = new CreateActivityRequest(Guid.NewGuid(), "Test Activity", 1000, 50, 25, 300);

        // Act
        var response = await this.client.PostAsJsonAsync("/api/activities", request);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task CreateReturnsCreatedCuandoProyectoExiste()
    {
        // Arrange
        var createProjectRequest = new CreateProjectRequest("Test Project", "Test Description");
        var projectResponse = await this.client.PostAsJsonAsync("/api/projects", createProjectRequest);
        var project = await projectResponse.Content.ReadFromJsonAsync<ProjectResponse>();
        var request = new CreateActivityRequest(project!.Id, "Test Activity", 1000, 50, 25, 300);

        // Act
        var response = await this.client.PostAsJsonAsync("/api/activities", request);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task GetByIdReturnsOkCuandoActividadExiste()
    {
        // Arrange
        var createProjectRequest = new CreateProjectRequest("Test Project", "Test Description");
        var projectResponse = await this.client.PostAsJsonAsync("/api/projects", createProjectRequest);
        var project = await projectResponse.Content.ReadFromJsonAsync<ProjectResponse>();
        var createActivityRequest = new CreateActivityRequest(project!.Id, "Test Activity", 1000, 50, 25, 300);
        var activityResponse = await this.client.PostAsJsonAsync("/api/activities", createActivityRequest);
        var activity = await activityResponse.Content.ReadFromJsonAsync<ActivityResponse>();

        // Act
        var response = await this.client.GetAsync($"/api/activities/{activity!.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task UpdateReturnsOkCuandoActividadExiste()
    {
        // Arrange
        var createProjectRequest = new CreateProjectRequest("Test Project", "Test Description");
        var projectResponse = await this.client.PostAsJsonAsync("/api/projects", createProjectRequest);
        var project = await projectResponse.Content.ReadFromJsonAsync<ProjectResponse>();
        var createActivityRequest = new CreateActivityRequest(project!.Id, "Test Activity", 1000, 50, 25, 300);
        var activityResponse = await this.client.PostAsJsonAsync("/api/activities", createActivityRequest);
        var activity = await activityResponse.Content.ReadFromJsonAsync<ActivityResponse>();
        var updateRequest = new UpdateActivityRequest("Updated Activity", 2000, 60, 30, 400);

        // Act
        var response = await this.client.PutAsJsonAsync($"/api/activities/{activity!.Id}", updateRequest);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task UpdateReturnsNotFoundCuandoActividadNoExiste()
    {
        // Arrange
        var updateRequest = new UpdateActivityRequest("Updated Activity", 2000, 60, 30, 400);

        // Act
        var response = await this.client.PutAsJsonAsync($"/api/activities/{Guid.NewGuid()}", updateRequest);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task DeleteReturnsNoContentCuandoActividadExiste()
    {
        // Arrange
        var createProjectRequest = new CreateProjectRequest("Test Project", "Test Description");
        var projectResponse = await this.client.PostAsJsonAsync("/api/projects", createProjectRequest);
        var project = await projectResponse.Content.ReadFromJsonAsync<ProjectResponse>();
        var createActivityRequest = new CreateActivityRequest(project!.Id, "Test Activity", 1000, 50, 25, 300);
        var activityResponse = await this.client.PostAsJsonAsync("/api/activities", createActivityRequest);
        var activity = await activityResponse.Content.ReadFromJsonAsync<ActivityResponse>();

        // Act
        var response = await this.client.DeleteAsync($"/api/activities/{activity!.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task DeleteReturnsNotFoundCuandoActividadNoExiste()
    {
        // Act
        var response = await this.client.DeleteAsync($"/api/activities/{Guid.NewGuid()}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
