namespace EVM.ProjectManagement.IntegrationTests.Projects;

using System.Net;
using System.Net.Http.Json;
using EVM.ProjectManagement.Application.Projects.DTOs;
using EVM.ProjectManagement.IntegrationTests.Fixtures;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Xunit;

public sealed class ProjectsControllerTests : IClassFixture<ApiWebApplicationFactory>
{
    private readonly HttpClient _client;

    public ProjectsControllerTests(ApiWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAll_WhenCalled_ReturnsOkWithProjectList()
    {
        // Act
        var response = await _client.GetAsync("/api/projects");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var projects = await response.Content.ReadFromJsonAsync<IReadOnlyList<ProjectResponse>>();
        projects.Should().NotBeNull();
    }

    [Fact]
    public async Task Create_WhenValidRequest_ReturnsCreatedWithProjectResponse()
    {
        // Arrange
        var request = new CreateProjectRequest("Test Project", "Test Description");

        // Act
        var response = await _client.PostAsJsonAsync("/api/projects", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location.Should().NotBeNull();
        var project = await response.Content.ReadFromJsonAsync<ProjectResponse>();
        project.Should().NotBeNull();
        project!.Name.Should().Be("Test Project");
        project.Description.Should().Be("Test Description");
    }

    [Fact]
    public async Task GetById_WhenProjectDoesNotExist_ReturnsNotFound()
    {
        // Act
        var response = await _client.GetAsync($"/api/projects/{Guid.NewGuid()}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        problemDetails.Should().NotBeNull();
        problemDetails!.Status.Should().Be(404);
    }

    [Fact]
    public async Task GetById_WhenProjectExists_ReturnsOkWithProjectResponse()
    {
        // Arrange
        var createRequest = new CreateProjectRequest("Test Project", "Test Description");
        var createResponse = await _client.PostAsJsonAsync("/api/projects", createRequest);
        var project = await createResponse.Content.ReadFromJsonAsync<ProjectResponse>();

        // Act
        var response = await _client.GetAsync($"/api/projects/{project!.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var retrievedProject = await response.Content.ReadFromJsonAsync<ProjectResponse>();
        retrievedProject.Should().NotBeNull();
        retrievedProject!.Id.Should().Be(project.Id);
        retrievedProject.Name.Should().Be("Test Project");
        retrievedProject.Description.Should().Be("Test Description");
    }

    [Fact]
    public async Task Update_WhenProjectExists_ReturnsOkWithUpdatedProjectResponse()
    {
        // Arrange
        var createRequest = new CreateProjectRequest("Test Project", "Test Description");
        var createResponse = await _client.PostAsJsonAsync("/api/projects", createRequest);
        var project = await createResponse.Content.ReadFromJsonAsync<ProjectResponse>();
        var updateRequest = new UpdateProjectRequest("Updated Project", "Updated Description");

        // Act
        var response = await _client.PutAsJsonAsync($"/api/projects/{project!.Id}", updateRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var updatedProject = await response.Content.ReadFromJsonAsync<ProjectResponse>();
        updatedProject.Should().NotBeNull();
        updatedProject!.Name.Should().Be("Updated Project");
        updatedProject.Description.Should().Be("Updated Description");
    }

    [Fact]
    public async Task Update_WhenProjectDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        var updateRequest = new UpdateProjectRequest("Updated Project", "Updated Description");

        // Act
        var response = await _client.PutAsJsonAsync($"/api/projects/{Guid.NewGuid()}", updateRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        problemDetails.Should().NotBeNull();
        problemDetails!.Status.Should().Be(404);
    }

    [Fact]
    public async Task Delete_WhenProjectExists_ReturnsNoContent()
    {
        // Arrange
        var createRequest = new CreateProjectRequest("Test Project", "Test Description");
        var createResponse = await _client.PostAsJsonAsync("/api/projects", createRequest);
        var project = await createResponse.Content.ReadFromJsonAsync<ProjectResponse>();

        // Act
        var response = await _client.DeleteAsync($"/api/projects/{project!.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Delete_WhenProjectDoesNotExist_ReturnsNotFound()
    {
        // Act
        var response = await _client.DeleteAsync($"/api/projects/{Guid.NewGuid()}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        problemDetails.Should().NotBeNull();
        problemDetails!.Status.Should().Be(404);
    }

    [Fact]
    public async Task Create_WhenRequestIsInvalid_ReturnsBadRequest()
    {
        // Arrange
        var request = new CreateProjectRequest("", "");

        // Act
        var response = await _client.PostAsJsonAsync("/api/projects", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        problemDetails.Should().NotBeNull();
        problemDetails!.Status.Should().Be(400);
    }

    [Fact]
    public async Task Update_WhenRequestIsInvalid_ReturnsBadRequest()
    {
        // Arrange
        var createRequest = new CreateProjectRequest("Test Project", "Test Description");
        var createResponse = await _client.PostAsJsonAsync("/api/projects", createRequest);
        var project = await createResponse.Content.ReadFromJsonAsync<ProjectResponse>();
        var updateRequest = new UpdateProjectRequest("", "");

        // Act
        var response = await _client.PutAsJsonAsync($"/api/projects/{project!.Id}", updateRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        problemDetails.Should().NotBeNull();
        problemDetails!.Status.Should().Be(400);
    }
}
